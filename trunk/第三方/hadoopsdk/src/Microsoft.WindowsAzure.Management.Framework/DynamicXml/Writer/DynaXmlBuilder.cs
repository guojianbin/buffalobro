﻿namespace Microsoft.WindowsAzure.Management.Framework.DynamicXml.Writer
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Used to build an Xml Document dynamically.
    /// </summary>
    public class DynaXmlBuilder : DynamicObject
    {
        private const string Xmlns = "http://www.w3.org/2000/xmlns/";
        private bool includeHeader;
        private XmlDocument document;
        private DynaXmlBuilderContext context;
        private Formatting xmlFormatting;
        private List<XmlAttribute> rootNodeNamespaceDefinitions = new List<XmlAttribute>();
        private readonly Dictionary<string, DynaXmlBuilderContext> setPoints = new Dictionary<string, DynaXmlBuilderContext>();

        /// <summary>
        /// Gets the current State of the building context.
        /// </summary>
        private DynaXmlBuilderState State
        {
            get { return this.context.State; }
        }

        /// <summary>
        /// Gets the current ancestor XmlNode for the builder.
        /// </summary>
        private XmlElement CurrentAncestorElement
        {
            get { return this.context.CurrentAncestorElement; }
        }

        /// <summary>
        /// Gets the current ancestor as an XmlNode.
        /// </summary>
        private XmlNode CurrentAncestorNode
        {
            get { return this.context.CurrentAncestorNode; }
        }

        /// <summary>
        /// Sets the last element that was created.
        /// </summary>
        private XmlElement LastCreated
        {
            // get { return this.context.LastCreated; }
            set { this.context.LastCreated = value; }
        }

        /// <summary>
        /// Gets the current DynaXmlNamespaceContext.
        /// </summary>
        private DynaXmlNamespaceContext CurrentNamespaceContext
        {
            get { return this.context.CurrentNamespaceContext; }
        }

        /// <summary>
        /// Initializes a new instance of the DynaXmlBuilder class.
        /// </summary>
        /// <param name="document">
        /// The XmlDocument to use for building.
        /// </param>
        /// <param name="includeHeader">
        /// A flag indicating that a header should be included.
        /// </param>
        /// <param name="xmlFormatting">
        /// The XmlFormatting style to use when writing the document.
        /// </param>
        internal DynaXmlBuilder(XmlDocument document, bool includeHeader, Formatting xmlFormatting)
        {
            this.xmlFormatting = xmlFormatting;
            this.includeHeader = includeHeader;
            this.context = new DynaXmlBuilderContext();
            this.document = document;
            // this.context.Push(this.document, DynaXmlBuilderState.ElementBuilder);
            this.context.Push(this.document, DynaXmlBuilderState.ElementListBuilder);
        }

        /// <summary>
        /// Creates a new instance of the DynaXmlBuilder class.
        /// This override allows the user to specify if a header should or should not be added.
        /// </summary>
        /// <param name="includeHeader">
        /// Specifies if a header (&lt;?xml ... &gt;) should be included.
        /// </param>
        /// <param name="xmlFormatting">
        /// The XmlFormatting style to use when writing the document.
        /// </param>
        /// <returns>
        /// A new DynaXmlBuilder object that can be used to build Xml.
        /// </returns>
        internal static dynamic Create(bool includeHeader, Formatting xmlFormatting)
        {
            var document = new XmlDocument();
            return new DynaXmlBuilder(document, includeHeader, xmlFormatting);
        }

        /// <summary>
        /// Creates a new instance of the DynaXmlBuilder class.
        /// This override will force the creation of an xml header.
        /// </summary>
        /// <returns>
        /// A new DynaXmlBuilder object that can be used to build Xml.
        /// </returns>
        public static dynamic Create()
        {
            return DynaXmlBuilder.Create(true, Formatting.Indented);
        }

        /// <summary>
        /// Creates a new element in the XmlDocument.
        /// </summary>
        /// <param name="name">
        /// The name of the element.
        /// </param>
        /// <returns>
        /// The XmlElement that was created.
        /// </returns>
        private XmlElement CreateElement(string name)
        {
            XmlElement element = null;
            var currentAlias = this.CurrentNamespaceContext.CurrentAlias;
            if (currentAlias.IsNotNullOrEmpty())
            {
                element = this.document.CreateElement(currentAlias,
                                                      name,
                                                      this.CurrentNamespaceContext.AliasTable[currentAlias]);
                // We've used the "CurrentAlias" so remove it as it's a (use once state);
                this.CurrentNamespaceContext.CurrentAlias = string.Empty;
                this.CurrentNamespaceContext.ApplyCurrentToAttributes = false;
            }
            else if (this.CurrentNamespaceContext.DefaultNamespace.IsNullOrEmpty())
            {
                element = this.document.CreateElement(name);
            }
            else
            {
                element = this.document.CreateElement(name, this.CurrentNamespaceContext.DefaultNamespace);
            }
            return element;
        }

        /// <summary>
        /// Used to manage members applied when in the Attribute state.
        /// </summary>
        /// <param name="name">
        /// The binder for the dynamic method get.
        /// </param>
        /// <returns>
        /// True if the operation is successful; otherwise, false. 
        /// If this method returns false, the run-time binder of the language determines the behavior
        /// (In most cases, a run-time exception is thrown).
        /// </returns>
        private bool AttributeBuilderGetMember(string name)
        {
            return this.AttributeBuilderInvokeMember(name, string.Empty);
        }

        /// <summary>
        /// Changes the current namespace alias for the current context.
        /// </summary>
        /// <param name="name">
        /// The alias for the new namespace.
        /// </param>
        /// <returns>
        /// Always true.
        /// </returns>
        private bool NamespaceBuilderGetMember(string name)
        {
            string xmlNamespace = null;
            if (!this.CurrentNamespaceContext.AliasTable.TryGetValue(name, out xmlNamespace))
            {
                throw new InvalidOperationException("An attempt was made to set the current XML namespace uri to a namespace that has not been defined.");
            }

            // NOTE: We pop the context first.  This is because we don't want to change the namespace
            // on the "NamespaceState" entry, but rather on the entry above it.
            this.context.Pop();
            // Change the current Alias.
            this.CurrentNamespaceContext.CurrentAlias = name;
            // If we are in an Attribute creation context, then we apply the namespace to the attribute.
            if (this.State == DynaXmlBuilderState.AttributeBuilder ||
                this.State == DynaXmlBuilderState.AttributeListBuilder)
            {
                this.CurrentNamespaceContext.ApplyCurrentToAttributes = true;
            }
            return true;
        }

        /// <summary>
        /// Changes the current namespace alias for the current context.
        /// </summary>
        /// <param name="name">
        /// The alias for the new namespace.
        /// </param>
        /// <param name="xmlNamespace">
        /// The namespace Url to apply.
        /// </param>
        /// <returns>
        /// Always true.
        /// </returns>
        private bool NamespaceBuilderInvokeMember(string name, string xmlNamespace)
        {
            // First check to make sure the namespace has not already been defined
            // on this element.  It's okay if it's in the current context, but it 
            // cant be on the AncestorElement.
            ICollection<XmlAttribute> currentAttributes;
            if (this.CurrentAncestorElement.IsNotNull())
            {
                currentAttributes = new List<XmlAttribute>();
                foreach (XmlAttribute attrib in this.CurrentAncestorElement.Attributes)
                {
                    currentAttributes.Add(attrib);
                }
            }
            else
            {
                currentAttributes = this.rootNodeNamespaceDefinitions;
            }

            XmlAttribute attribute = (from a in currentAttributes
                                     where a.Name == name &&
                                           a.NamespaceURI == Xmlns
                                    select a).FirstOrDefault();
            if (attribute.IsNotNull())
            {
                attribute.Value = xmlNamespace;
            }
            else
            {
                attribute = this.CreateNamespace(name, xmlNamespace);
                if (this.CurrentAncestorElement.IsNotNull())
                {
                    this.CurrentAncestorElement.Attributes.Append(attribute);
                }
                else
                {
                    this.rootNodeNamespaceDefinitions.Add(attribute);
                }
            }

            // Again we pop the context first.
            this.context.Pop();
            // Then we make the changes in the Namespace area of the context.
            this.CurrentNamespaceContext.AliasTable[name] = xmlNamespace;
            return true;
        }

        /// <summary>
        /// Used to manage the creation of Elements against the a child element within the Xml document.
        /// </summary>
        /// <param name="name">
        /// The name of the member.
        /// </param>
        /// <returns>
        /// Always true.
        /// </returns>
        private bool ElementBuilderGetMember(string name)
        {
            // Determine if we are at the root level and if we are trying to 
            // create more than one node.
            if (this.CurrentAncestorNode == this.document && this.document.ChildNodes.Count != 0)
            {
                throw new InvalidOperationException("An Xml Document may not have more than one root element.");
            }

            // If we are in a list builder, append the current element.
            if (this.State == DynaXmlBuilderState.ElementListBuilder ||
                this.State == DynaXmlBuilderState.LiteralElementBuilder)
            {
                var element = this.CreateElement(name);
                this.CurrentAncestorNode.AppendChild(element);
                // update the last created in case a new list is needed under this element.
                this.LastCreated = element;
            }
            else
            {
                // Push element updates both the last created element and the ancestor.
                // This is because a new ancestor is by definition the last created.
                this.context.PushElement(this.CreateElement(name));
            }
                
            if (this.State == DynaXmlBuilderState.LiteralElementBuilder)
            {
                this.context.Pop();
            }
            return true;
        }

        /// <summary>
        /// Saves the Xml to a given file.
        /// </summary>
        /// <param name="fileName">
        /// The name of the file to save to.
        /// </param>
        private void Save(string fileName)
        {
            using (var stream = new FileStream(fileName,
                                               FileMode.Create,
                                               FileAccess.Write,
                                               FileShare.Read))
            {
                this.Save(stream);
            }
        }

        /// <summary>
        /// Saves the Xml to a given stream.
        /// </summary>
        /// <param name="stream">
        /// The stream to save to.
        /// </param>
        private void Save(Stream stream)
        {
            // Use XmlWriterSettings to control the Xml construction.
            XmlWriterSettings settings = new XmlWriterSettings();
            // If indentation was request, then specify indentation.
            settings.Indent = this.xmlFormatting == Formatting.Indented;
            // Omit the header if include header is false.
            settings.OmitXmlDeclaration = !this.includeHeader;

            // Use a temporary memory stream as the writer will always dispose the stream.
            using (MemoryStream temp = new MemoryStream())
            using (var streamWriter = new StreamWriter(temp))
            using (var xmlWriter = XmlTextWriter.Create(streamWriter, settings))
            {
                this.document.Save(xmlWriter);
                // Copy the content into the actual stream supplied.
                temp.Position = 0;
                temp.CopyTo(stream);
            }
        }

        /// <summary>
        /// Creates an Xml Attribute.
        /// </summary>
        /// <param name="name">
        /// The name of the attribute.
        /// </param>
        /// <param name="value">
        /// The value of the attribute.
        /// </param>
        /// <returns>
        /// The new attribute.
        /// </returns>
        private XmlAttribute CreateAttribute(string name, string value)
        {
            XmlAttribute retval;
            var currentAlias = this.CurrentNamespaceContext.CurrentAlias;
            if (!this.CurrentNamespaceContext.ApplyCurrentToAttributes)
            {
                retval = this.document.CreateAttribute(name);
            }
            else
            {
                retval = this.document.CreateAttribute(currentAlias,
                                                       name,
                                                       this.CurrentNamespaceContext.AliasTable[currentAlias]);
                // We've used the current alias so remove it as it is a (use once state)
                this.CurrentNamespaceContext.CurrentAlias = string.Empty;
                this.CurrentNamespaceContext.ApplyCurrentToAttributes = false;
            }
            retval.Value = value;
            return retval;
        }

        /// <summary>
        /// Creates a new XmlNamespace element.
        /// </summary>
        /// <param name="prefix">The prefix for the namespace.</param>
        /// <param name="xmlNamespace">The namespace.</param>
        /// <returns>A new XmlAttribute representing the namespace.</returns>
        private XmlAttribute CreateNamespace(string prefix, string xmlNamespace)
        {
            XmlAttribute namespaceDefinition = this.document.CreateAttribute("xmlns", prefix, Xmlns);
            this.CurrentNamespaceContext.AliasTable.Add(prefix, xmlNamespace);
            namespaceDefinition.Value = xmlNamespace;
            return namespaceDefinition;
        }

        /// <summary>
        /// Gets a value indicating whether we are in a sate of processing an element.
        /// </summary>
        private bool IsElementState
        {
            get
            {
                return // this.State == DynaXmlBuilderState.ElementBuilder ||
                       this.State == DynaXmlBuilderState.ElementListBuilder ||
                       this.State == DynaXmlBuilderState.LiteralElementBuilder;
            }
        }

        /// <summary>
        /// Gets a value indicating whether we are in a state of processing a literal.
        /// </summary>
        private bool IsLiteratState
        {
            get
            {
                return this.State == DynaXmlBuilderState.LiteralElementBuilder ||
                       this.State == DynaXmlBuilderState.LiteralAttributeBuilder;
            }
        }

        /// <summary>
        /// Changes the default namespace to apply to all newly created elements.
        /// </summary>
        /// <param name="xmlNamespace">
        /// The new xml namespace.
        /// </param>
        private void SetDefaultNamespace(string xmlNamespace)
        {
            this.CurrentNamespaceContext.DefaultNamespace = xmlNamespace;
        }

        /// <inheritdoc />
        /// This method handles property gets.
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // A binder must have been supplied, the runtime will always do this.
            if (binder.IsNull())
            {
                throw new ArgumentNullException("binder");
            }

            // This is an "interface flowing" construct, so the result is always this.
            result = this;

            // Starts a Literal block
            if (binder.Name == "l" && !this.IsLiteratState)
            {
                switch (this.State)
                {
                    // If we are in an Attribute List builder, we push state.
                    case DynaXmlBuilderState.AttributeListBuilder:
                        this.context.PushState(DynaXmlBuilderState.LiteralAttributeBuilder);
                        break;
                    // If we are in a single attribute builder we change state (so the pop takes 
                    // us out of the attribute building)
                    case DynaXmlBuilderState.AttributeBuilder:
                        this.context.ChangeState(DynaXmlBuilderState.LiteralAttributeBuilder);
                        break;
                    // If we are in an Element or an Element List builder we push state.
                    case DynaXmlBuilderState.ElementListBuilder:
                        this.context.PushState(DynaXmlBuilderState.LiteralElementBuilder);
                        break;
                }
                return true;
            }

            // Starts an Attribute block which allows for an attribute to be added.
            if (binder.Name == "at" && !this.IsLiteratState)
            {
                this.context.PushState(DynaXmlBuilderState.AttributeBuilder);
                return true;
            }

            if (binder.Name == "xmlns" && !this.IsLiteratState)
            {
                this.context.PushState(DynaXmlBuilderState.NamespaceBuilder);
                return true;
            }

            // Starts a list (which is multiple elements or attributes in a row)
            if (binder.Name == "b")
            {
                if (this.State == DynaXmlBuilderState.ElementListBuilder)
                {
                    // Enter a child list of the parent.
                    this.context.PushState(DynaXmlBuilderState.ElementListBuilder);
                    return true;
                }
                if (this.State == DynaXmlBuilderState.AttributeBuilder)
                {
                    // Change state to attribute list builder.
                    this.context.ChangeState(DynaXmlBuilderState.AttributeListBuilder);
                    return true;
                }
            }

            // Ends a list (multiple elements or attributes in a row).
            if (binder.Name == "d" && 
                (this.State == DynaXmlBuilderState.ElementListBuilder ||
                 this.State == DynaXmlBuilderState.AttributeListBuilder))
            {
                this.context.Pop();
                return true;
            }

            switch (this.State)
            {
                case DynaXmlBuilderState.NamespaceBuilder:
                    return this.NamespaceBuilderGetMember(binder.Name);
                case DynaXmlBuilderState.ElementListBuilder:
                case DynaXmlBuilderState.LiteralElementBuilder:
                    return this.ElementBuilderGetMember(binder.Name);
                case DynaXmlBuilderState.AttributeBuilder:
                case DynaXmlBuilderState.LiteralAttributeBuilder:
                case DynaXmlBuilderState.AttributeListBuilder:
                    return this.AttributeBuilderGetMember(binder.Name);
            }
            // Otherwise call into base.
            // Base currently does nothing (but returns false and fails), but a new class could be interposed.
            return base.TryGetMember(binder, out result);
        }

        /// <inheritdoc />
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            // For interface flowing, the resulting object is always this object.
            result = this;
            if (binder == null)
            {
                throw new ArgumentNullException("binder");
            }

            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            // Ends the line of chaining.
            if (binder.Name == "End" && !this.IsLiteratState)
            {
                return true;
            }

            if (args.Length == 0 || args[0].IsNull())
            {
                if (this.IsElementState)
                {
                    this.ElementBuilderInvokeMember(binder.Name, string.Empty);
                }
                throw new ArgumentOutOfRangeException("args");
            }

            // Get the value of the first argument as a string.
            var arg0 = args[0].ToString();

            // Special method that saves the content of the XML.
            if (binder.Name == "Save" && !this.IsLiteratState)
            {
                var asStream = args[0] as Stream;
                if (asStream.IsNotNull())
                {
                    this.Save(asStream);
                    return true;
                }
                this.Save(arg0);
                return true;
            }

            // start a "save point" this can be used to go back to the midle of the document later.
            if (binder.Name == "sp" && !this.IsLiteratState)
            {
                this.setPoints[arg0] = new DynaXmlBuilderContext(this.context);
                return true;
            }

            // reload previously created save point.
            if (binder.Name == "rp" && !this.IsLiteratState)
            {
                this.context = this.setPoints[arg0];
                return true;
            }

            // Creates a CDATA block with the content supplied.
            if (binder.Name == "cdata" && !this.IsLiteratState)
            {
                return this.CDataInvokeMember(arg0);
            }

            // Sets the default namespace for the current element.
            if (binder.Name == "xmlns" &&
                !this.IsLiteratState)
            {
                this.SetDefaultNamespace(arg0);
                return true;
            }

            switch (this.State)
            {
                case DynaXmlBuilderState.NamespaceBuilder:
                    return this.NamespaceBuilderInvokeMember(binder.Name, arg0);
                case DynaXmlBuilderState.AttributeBuilder:
                case DynaXmlBuilderState.AttributeListBuilder:
                case DynaXmlBuilderState.LiteralAttributeBuilder:
                    return this.AttributeBuilderInvokeMember(binder.Name, arg0);
                case DynaXmlBuilderState.ElementListBuilder:
                case DynaXmlBuilderState.LiteralElementBuilder:
                    return this.ElementBuilderInvokeMember(binder.Name, arg0);
                //case DynaXmlBuilderState.NamespaceBuilder:
                //    return this.NamespaceBuilderInvokeMember(binder, args);
            }
            return base.TryInvokeMember(binder, args, out result);
        }

        /// <summary>
        /// Adds an element with content to the tree.
        /// </summary>
        /// <param name="name">
        /// The name of the element to add.
        /// </param>
        /// <param name="value">
        /// The value to place as the text of the element.
        /// </param>
        /// <returns>
        /// Always true.
        /// </returns>
        private bool ElementBuilderInvokeMember(string name, string value)
        {
            // Determine if we are at the root level and if we are trying to 
            // create more than one node.
            if (this.CurrentAncestorNode == this.document && this.document.ChildNodes.Count != 0)
            {
                throw new InvalidOperationException("An Xml Document may not have more than one root element.");
            }

            XmlElement element = this.CreateElement(name);
            element.InnerText = value ?? string.Empty;
            this.CurrentAncestorNode.AppendChild(element);
            if (this.State == DynaXmlBuilderState.LiteralElementBuilder)
            {
                this.context.Pop();
            }
            return true;
        }

        /// <summary>
        /// Creates an Attribute as a component of the current element.
        /// </summary>
        /// <param name="name">
        /// The name of the attribute.
        /// </param>
        /// <param name="value">
        /// The value for the attribute.
        /// </param>
        /// <returns>
        /// Always true.
        /// </returns>
        private bool AttributeBuilderInvokeMember(string name, string value)
        {
            // Determine if we are at the root level and if we are trying to 
            // create more than one node.
            if (this.CurrentAncestorNode == this.document && this.document.ChildNodes.Count != 0)
            {
                throw new InvalidOperationException("Attributes can not be added before a root element is defined.");
            }

            XmlNode element = this.CurrentAncestorElement;
            if (element.Attributes != null)
            {
                element.Attributes.Append(this.CreateAttribute(name, value));
            }
            // If we are NOT in a AttributeListBuild, we pop the state
            // if we are in a list attribute build, we do not.
            if (this.State != DynaXmlBuilderState.AttributeListBuilder)
            {
                this.context.Pop();
            }
            return true;
        }

        /// <summary>
        /// Creates a CDATA block with the text supplied.
        /// </summary>
        /// <param name="content">
        /// The content to place in the CDATA block.
        /// </param>
        /// <returns>
        /// Always true.
        /// </returns>
        private bool CDataInvokeMember(string content)
        {
            // Determine if we are at the root level and if we are trying to 
            // create more than one node.
            if (this.CurrentAncestorNode == this.document && this.document.ChildNodes.Count != 0)
            {
                throw new InvalidOperationException("CDATA nodes can not be part of the root xml structure.");
            }

            XmlCDataSection cdata = this.document.CreateCDataSection(content);
            this.CurrentAncestorNode.AppendChild(cdata);
            return true;
        }

    }
}
