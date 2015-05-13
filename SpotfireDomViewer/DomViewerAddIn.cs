namespace SpotfireDomViewer
{
    using Spotfire.Dxp.Application.Extension;

    /// <summary>
    /// The dom viewer add in.
    /// </summary>
    public class DomViewerAddIn : AddIn
    {
        protected override void RegisterTools(ToolRegistrar registrar)
        {
            base.RegisterTools(registrar);
            registrar.Register(new DomViewerTool());
        }
    }
}
