using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EnumGenerator;

[Generator]
public class EnumGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Add the marker attribute
        // context.RegisterPostInitializationOutput(ctx => ctx.AddSource(
        //     "EnumExtensionsAttribute.g.cs", 
        //     SourceText.From(SourceGenerationHelper.Attribute, Encoding.UTF8)));

        // Do a simple filter for enums
        var enumDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider<EnumDeclarationSyntax>(
                static (s, _) => IsSyntaxTargetForGeneration(s), // select enums with attributes
                static (ctx, _) =>
                    GetSemanticTargetForGeneration(ctx)) // sect the enum with the [EnumExtensions] attribute
            .Where(static m => m is not null)!; // filter out attributed enums that we don't care about

        // Combine the selected enums with the `Compilation`
        IncrementalValueProvider<(Compilation, ImmutableArray<EnumDeclarationSyntax>)> compilationAndEnums
            = context.CompilationProvider.Combine(enumDeclarations.Collect());

        // Generate the source using the compilation and enums
        context.RegisterSourceOutput(compilationAndEnums,
            static (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    private static bool IsSyntaxTargetForGeneration(SyntaxNode node)
    {
        return node is EnumDeclarationSyntax { AttributeLists.Count: > 0 };
    }


    private static EnumDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
    {
        // we know the node is a EnumDeclarationSyntax thanks to IsSyntaxTargetForGeneration
        var enumDeclarationSyntax = (EnumDeclarationSyntax)context.Node;

        // loop through all the attributes on the method
        foreach (var attributeListSyntax in enumDeclarationSyntax.AttributeLists)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                {
                    // weird, we couldn't get the symbol, ignore it
                    continue;
                }

                var attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                var fullName = attributeContainingTypeSymbol.ToDisplayString();

                // Is the attribute the [EnumExtensions] attribute?
                if (fullName == "EnumGenerators.EnumAttrAttribute")
                {
                    // return the enum
                    return enumDeclarationSyntax;
                }
            }
        }

        // we didn't find the attribute we were looking for
        return null;
    }

    private static void Execute(Compilation compilation, ImmutableArray<EnumDeclarationSyntax> enums,
        SourceProductionContext context)
    {
        if (enums.IsDefaultOrEmpty)
        {
            // nothing to do yet
            return;
        }

        // I'm not sure if this is actually necessary, but `[LoggerMessage]` does it, so seems like a good idea!
        var distinctEnums = enums.Distinct();

        // Convert each EnumDeclarationSyntax to an EnumToGenerate
        var enumsToGenerate =
            GetTypesToGenerate(compilation, distinctEnums, context.CancellationToken);

        // If there were errors in the EnumDeclarationSyntax, we won't create an
        // EnumToGenerate for it, so make sure we have something to generate
        if (enumsToGenerate.Count > 0)
        {
            // generate the source code and add it to the output
            var result = GenerateExtensionClass(enumsToGenerate);
            context.AddSource("EnumGenerators.g.cs", SourceText.From(result, Encoding.UTF8));
        }
    }

    private static List<EnumToGenerate> GetTypesToGenerate(Compilation compilation,
        IEnumerable<EnumDeclarationSyntax> enums, CancellationToken ct)
    {
        // Create a list to hold our output
        var enumsToGenerate = new List<EnumToGenerate>();
        // Get the semantic representation of our marker attribute 
        var enumAttribute = compilation.GetTypeByMetadataName("EnumGenerators.EnumAttrAttribute");

        if (enumAttribute == null)
        {
            // If this is null, the compilation couldn't find the marker attribute type
            // which suggests there's something very wrong! Bail out..
            return enumsToGenerate;
        }

        foreach (var enumDeclarationSyntax in enums)
        {
            // stop if we're asked to
            ct.ThrowIfCancellationRequested();

            // Get the semantic representation of the enum syntax
            var semanticModel = compilation.GetSemanticModel(enumDeclarationSyntax.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(enumDeclarationSyntax) is not INamedTypeSymbol enumSymbol)
            {
                // something went wrong, bail out
                continue;
            }

            // Get the full type name of the enum e.g. Colour, 
            // or OuterClass<T>.Colour if it was nested in a generic type (for example)
            var enumName = enumSymbol.ToString();

            // Get all the members in the enum
            var enumMembers = enumSymbol.GetMembers();
            var members = new List<string>(enumMembers.Length);

            // Get all the fields from the enum, and add their name to the list
            foreach (var member in enumMembers)
            {
                if (member is IFieldSymbol field && field.ConstantValue is not null)
                {
                    members.Add(member.Name);
                }
            }

            // Create an EnumToGenerate for use in the generation phase
            enumsToGenerate.Add(new EnumToGenerate(enumName, members));
        }

        return enumsToGenerate;
    }

    public static string GenerateExtensionClass(List<EnumToGenerate> enumsToGenerate)
    {
        var sb = new StringBuilder();
        sb.Append(@"
namespace EnumGenerators.EnumGenerators
{
    public static class EnumExtensions
    {");
        foreach (var enumToGenerate in enumsToGenerate)
        {
            sb.Append(@"
        public static string ToStringFast(this ").Append(enumToGenerate.Name).Append(@" value)
            => value switch
            {");
            foreach (var member in enumToGenerate.Values)
            {
                sb.Append(@"
                ").Append(enumToGenerate.Name).Append('.').Append(member)
                    .Append(" => nameof(")
                    .Append(enumToGenerate.Name).Append('.').Append(member).Append("),");
            }

            sb.Append(@"
                _ => value.ToString(),
            };");
        }

        sb.Append(@"
    }
}");

        return sb.ToString();
    }
}