namespace SpotfireDomViewer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Spotfire.Dxp.Application;
    using Spotfire.Dxp.Application.Extension;

    /// <summary>
    /// The dom viewer tool.
    /// </summary>
    internal class DomViewerTool : CustomTool<AnalysisApplication>
    {
        public DomViewerTool()
            : base("DOM Viewer...")
        {
        }

        protected override void ExecuteCore(AnalysisApplication context)
        {
            var vm = new DomViewerViewModel(context);
            var win = new DomViewerWindow(vm) { Title = context.ToString() };
            win.Show();
        }

        protected override bool IsEnabledCore(AnalysisApplication context)
        {
            return true;
        }

        protected override bool IsVisibleCore(AnalysisApplication context)
        {
            return true;
        }
    }
}
