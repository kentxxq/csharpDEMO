// See https://aka.ms/new-console-template for more information

using MyLDAP;

// https://www.forumsys.com/2022/05/10/online-ldap-test-server/
var result = LDAP.VerifyLdapConnection("ldap.forumsys.com", 389, @"cn=read-only-admin,dc=example,dc=com", "password");
Console.WriteLine(result);