using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;


// 文档地址
// https://docs.sixlabors.com/articles/imagesharp/gettingstarted.html

const string folderPath = @"";

using (Image image = Image.Load(Path.Combine(folderPath,"PixPin_2024-06-27_11-54-22.gif")))
{

    // image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
    image.Save(Path.Combine(folderPath,"result.gif"));
}

Console.WriteLine("done");
