// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DomNode.cs" company="">
//   
// </copyright>
// <summary>
//   The dom node.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace SpotfireDomViewer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The node types.
    /// </summary>
    public enum NodeTypes
    {
        /// <summary>
        /// The field.
        /// </summary>
        Field,

        /// <summary>
        /// The property.
        /// </summary>
        Property,

        /// <summary>
        /// The item.
        /// </summary>
        Item,

        /// <summary>
        /// The method.
        /// </summary>
        Method
    }

    /// <summary>
    ///     The dom node.
    /// </summary>
    public class DomNode : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        ///     The dom object.
        /// </summary>
        private object domObj;

        /// <summary>
        ///     The name.
        /// </summary>
        private string name;

        /// <summary>
        ///     The node type.
        /// </summary>
        private Type nodeType;

        /// <summary>
        /// The type.
        /// </summary>
        private NodeTypes type;

        /// <summary>
        /// The can invoke.
        /// </summary>
        private bool canInvoke;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DomNode"/> class.
        /// </summary>
        /// <param name="obj">
        /// The object.
        /// </param>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public DomNode(object obj, NodeTypes type, string name = "")
        {
            this.DomObject = obj;
            this.Type = type;
            this.Name = string.IsNullOrEmpty(name) ? obj.ToString() : name;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///     The property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the children.
        /// </summary>
        public IEnumerable<DomNode> Children
        {
            get
            {
                if (this.DomObject != null && this.NodeType != null && !this.NodeType.IsGenericType && !this.NodeType.IsGenericParameter)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(this.NodeType))
                    {
                        if (this.DomObject as IEnumerable != null)
                        {
                            foreach (object o in this.DomObject as IEnumerable)
                            {
                                yield return new DomNode(o, NodeTypes.Item, o.ToString());
                            }
                        }
                    }
                    else
                    {
                        foreach (var f in this.NodeType.GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                        {
                            object v = null;
                            try
                            {
                                v = f.GetValue(this.DomObject);
                            }
                            catch
                            {
                            }

                            if (v != null) { yield return new DomNode(v, NodeTypes.Field, f.Name); }

                        }

                        foreach (var p in this.NodeType.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                        {
                            object v = null;
                            try
                            {
                                v = p.GetValue(this.DomObject, null);
                            }
                            catch
                            {
                            }

                            if (v != null) { yield return new DomNode(v, NodeTypes.Property, p.Name); }
                        }

                        foreach (var m in this.NodeType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                        {
                            if (!m.GetParameters().Any() && m.ReturnType != typeof(void) && !m.IsGenericMethod && !m.ContainsGenericParameters && !m.ReturnType.IsGenericParameter && !m.ReturnType.IsGenericType)
                            {
                                yield return new MethodNode(this.DomObject, m);
                            }
                            else
                            {
                                yield return new DomNode(null, NodeTypes.Method, m.ToString());
                            }
                        }
                    }
                }
            }
        }

        public bool CanInvoke
        {
            get
            {
                return canInvoke;
            }

            set
            {
                canInvoke = value;
                this.OnNotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the dom object.
        /// </summary>
        public object DomObject
        {
            get
            {
                return this.domObj;
            }

            set
            {
                this.domObj = value;
                if (value != null)
                {
                    this.NodeType = value.GetType();
                }

                this.OnNotifyPropertyChanged();
                this.OnNotifyPropertyChanged("Value");
            }
        }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.name = value;
                this.OnNotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the node type.
        /// </summary>
        public Type NodeType
        {
            get
            {
                return this.nodeType;
            }

            set
            {
                this.nodeType = value;
                this.OnNotifyPropertyChanged();
                this.OnNotifyPropertyChanged("Children");
            }
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public NodeTypes Type
        {
            get
            {
                return this.type;
            }

            set
            {
                this.type = value;
                this.OnNotifyPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        public string Value
        {
            get
            {
                if (this.DomObject == null)
                {
                    return "null";
                }

                if (typeof(string).IsAssignableFrom(this.NodeType))
                {
                    return "\"" + this.DomObject + "\"";
                }

                if (typeof(IEnumerable).IsAssignableFrom(this.NodeType))
                {
                    var enumerable = this.DomObject as IEnumerable;
                    if (enumerable != null)
                    {
                        return string.Format("[count: {0}]", enumerable.Cast<object>().Count());
                    }
                }

                return this.DomObject.ToString();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on notify property changed.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        private void OnNotifyPropertyChanged([CallerMemberName] string name = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        #endregion
    }
}