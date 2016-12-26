//using System;
//using System.Net.Http.Formatting;
//using Umbraco.Core;
//using Umbraco.Web.Models.Trees;
//using Umbraco.Web.Mvc;
//using Umbraco.Web.Trees;

//namespace ScottybonsMVC.App_Plugins.ScottybonsAdmin.App_Code
//{
//    /// <summary>
//    /// Summary description for UmbExtendTreeController
//    /// </summary>

//    //[Tree("content", "ScottybonsAdminDashboard", "Order Dashboard")]
//    //[PluginController("ScottybonsAdmin")]
//    public class ScottybonsAdminController : TreeController
//    {
//        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
//        {
//            // check if we're rendering the root node's children
//            if (id == Constants.System.Root.ToInvariantString())
//            {
//                // empty tree
//                var tree = new TreeNodeCollection();
//                // but if we wanted to add nodes - 
//                /*  var tree = new TreeNodeCollection
//            {
//                CreateTreeNode("1", id, queryStrings, "My Node 1"), 
//                CreateTreeNode("2", id, queryStrings, "My Node 2"), 
//                CreateTreeNode("3", id, queryStrings, "My Node 3")
//            };*/
//                return tree;
//            }
//            // this tree doesn't support rendering more than 1 level
//            throw new NotSupportedException();
//        }


//        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
//        {
//            // create my menu item "Order Dashboard"
//            var menu = new MenuItemCollection();
//            // duplicate this section for more than one icon
//            var m = new MenuItem("Order", "Order Dashboard");
//            m.Icon = "settings-alt";
//            menu.Items.Add(m);

//            return menu;
//        }
//    }
//}
