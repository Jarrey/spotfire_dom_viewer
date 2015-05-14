namespace SpotfireDomViewer
{
    using System.Reflection;

    public class MethodNode : DomNode
    {
        private object parentObject;

        private MethodInfo method;

        public MethodNode(object parentObj, MethodInfo m)
            : base(null, NodeTypes.Method, m.ToString())
        {
            parentObject = parentObj;
            method = m;
            CanInvoke = true;
        }
        public string Value
        {
            get
            {
                // var v = this.Invoke();
                // return v == null ? string.Empty : v.ToString();
                return "Please invoke this method in right panel...";
            }
        }

        public object Invoke()
        {
            if (method != null)
            {
                return method.Invoke(parentObject, null);
            }

            return null;
        }
    }
}
