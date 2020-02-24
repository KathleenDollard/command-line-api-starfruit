// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace System.CommandLine.ReflectionModel.XmlDocument
{
    public class TypeHelpMetadata
    {
        public string Description { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> PropertyDescriptions { get; } = new Dictionary<string, string>();
    }
}
