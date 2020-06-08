using System;
using System.Collections.Generic;
using System.CommandLine.GeneralAppModel;
using System.Text;

namespace Playground
{
    public class TemplateAttribute : Attribute { }

    public enum Authorization
    {
        None,          // No authentication
        Individual,    // Individual authentication
        IndividualB2C, // Individual authentication with Azure AD B2C
        SingleOrg,     // Organizational authentication for a single tenant
        MultiOrg,      // Organizational authentication for multiple tenants
        Windows,       // Windows authentication

    }

    [Template]
    public class WebAppTemplate
    {
        public string Invoke([Option(Aliases = new string[] { "au", "auth" }, Description ="The type of authentication to use" )]
                              Authorization authorization = Authorization.None,

                              [Option(Description ="Directory B2C instance to connect to(use with IndividualB2C auth)")]
                              [DefaultValue("https://login.microsoftonline.com/tfp/")]
                              string AadB2cInstance)
        {
            return "Template Result";
        }

 
    }
}
