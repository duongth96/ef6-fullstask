//Contributor : MVCContrib

using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using GE.Warehouse.Web.Framework.UI.Paging;

namespace GE.Warehouse.Web.Framework
{
    /// <summary>
    /// Renders a pager component from an IPageableModel datasource.
    /// </summary>
    public partial class Pager : IHtmlString
    {
        protected readonly IPageableModel model;
        protected readonly ViewContext viewContext;
        protected string pageQueryName = "page";
        protected bool showTotalSummary = true;
        protected bool showPagerItems = true;
        protected bool showFirst = true;
        protected bool showPrevious = true;
        protected bool showNext = true;
        protected bool showLast = true;
        protected bool showIndividualPages = true;
        protected int individualPagesDisplayedCount = 5;
        protected Func<int, string> urlBuilder;
        protected IList<string> booleanParameterNames;

        public Pager(IPageableModel model, ViewContext context)
        {
            this.model = model;
            this.viewContext = context;
            this.urlBuilder = CreateDefaultUrl;
            this.booleanParameterNames = new List<string>();
        }

        protected ViewContext ViewContext
        {
            get { return viewContext; }
        }

        public Pager QueryParam(string value)
        {
            this.pageQueryName = value;
            return this;
        }
        public Pager ShowTotalSummary(bool value)
        {
            this.showTotalSummary = value;
            return this;
        }
        public Pager ShowPagerItems(bool value)
        {
            this.showPagerItems = value;
            return this;
        }
        public Pager ShowFirst(bool value)
        {
            this.showFirst = value;
            return this;
        }
        public Pager ShowPrevious(bool value)
        {
            this.showPrevious = value;
            return this;
        }
        public Pager ShowNext(bool value)
        {
            this.showNext = value;
            return this;
        }
        public Pager ShowLast(bool value)
        {
            this.showLast = value;
            return this;
        }
        public Pager ShowIndividualPages(bool value)
        {
            this.showIndividualPages = value;
            return this;
        }
        public Pager IndividualPagesDisplayedCount(int value)
        {
            this.individualPagesDisplayedCount = value;
            return this;
        }
        public Pager Link(Func<int, string> value)
        {
            this.urlBuilder = value;
            return this;
        }
        //little hack here due to ugly MVC implementation
        //find more info here: http://www.mindstorminteractive.com/blog/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
        public Pager BooleanParameterName(string paramName)
        {
            booleanParameterNames.Add(paramName);
            return this;
        }

        public override string ToString()
        {
            return ToHtmlString();
        }
        public string ToHtmlString()
        {
            if (model.TotalItems == 0)
                return null;
            //var localizationService = EngineContext.Current.Resolve<ILocalizationService>();


            var links = new StringBuilder();
            if (showPagerItems && (model.TotalPages > 1))
            {
                links.AppendFormat("<div class=\"row\">");
                if (showTotalSummary && (model.TotalPages > 0))
                {
                    var lastEntry = (model.PageSize * model.PageIndex < model.TotalItems) ? model.PageSize * model.PageIndex : model.TotalItems;
                    links.AppendFormat("<div class=\"col-md-6\"><div class=\"dataTables_info\"> Showing {0} to {1} of {2} entries </div> </div>", (model.PageIndex - 1) * model.PageSize + 1, lastEntry, model.TotalItems);
                }

                links.Append("<div class=\"span6\"><div class=\"dataTables_paginate paging_bootstrap pagination\"><ul>");
                if (showFirst)
                {
                    if ((model.PageIndex >= 3) && (model.TotalPages > individualPagesDisplayedCount))
                    {
                        if (showIndividualPages)
                        {
                            links.Append("&nbsp;");
                        }
                        links.Append("<li class=\"first\">");
                        links.Append(CreatePageLink(1, "<<"));
                        links.Append("</li>");
                        //if ((showIndividualPages || (showPrevious && (model.PageIndex > 0))) || showLast)
                        //{
                        //    links.Append("&nbsp;...&nbsp;");
                        //}
                    }
                }
                if (showPrevious)
                {
                    if (model.PageIndex > 1)
                    {
                        links.Append("<li class=\"prev\">");
                        links.Append(CreatePageLink(model.PageIndex - 1, "<"));

                        if ((showIndividualPages || showLast) || (showNext && ((model.PageIndex + 1) < model.TotalPages)))
                        {
                            links.Append("&nbsp;");
                        }
                        links.Append("</li>");
                    }
                }
                if (showIndividualPages)
                {
                    int firstIndividualPageIndex = GetFirstIndividualPageIndex();
                    int lastIndividualPageIndex = GetLastIndividualPageIndex();
                    for (int i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
                    {
                        if (model.PageIndex == i + 1)
                        {
                            links.Append("<li class=\"active\">");
                            links.Append(CreatePageLink(i + 1, (i + 1).ToString()));
                            links.Append("</li>");
                        }
                        else
                        {
                            links.Append("<li>");
                            links.Append(CreatePageLink(i + 1, (i + 1).ToString()));
                            links.Append("</li>");
                        }
                        if (i < lastIndividualPageIndex)
                        {
                            links.Append("&nbsp;");
                        }
                    }
                }
                if (showNext)
                {
                    if ((model.PageIndex + 1) < model.TotalPages)
                    {
                        links.Append("<li class=\"next\">");
                        if (showIndividualPages)
                        {
                            links.Append("&nbsp;");
                        }

                        links.Append(CreatePageLink(model.PageIndex + 1, ">"));
                        links.Append("</li>");
                    }
                }
                if (showLast)
                {
                    if (((model.PageIndex + 3) < model.TotalPages) && (model.TotalPages > individualPagesDisplayedCount))
                    {
                        //if (showIndividualPages || (showNext && ((model.PageIndex + 1) < model.TotalPages)))
                        //{
                        //    links.Append("&nbsp;...&nbsp;");
                        //}
                        links.Append("<li class=\"last\">");
                        links.Append(CreatePageLink(model.TotalPages, ">>"));
                        links.Append("</li>");
                    }
                }

                links.Append("</ul></div></div></div>");
            }

            return links.ToString();
        }

        public MvcHtmlString ToTableFooterHtmlString()
        {
            if (model.TotalItems == 0)
                return null;
            //var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

            var links = new StringBuilder();
            if (showPagerItems && (model.TotalPages > 1))
            {
                links.AppendFormat("<div class=\"dt-toolbar-footer\">");
                if (showTotalSummary && (model.TotalPages > 0))
                {
                    var lastEntry = (model.PageSize * model.PageIndex < model.TotalItems) ? model.PageSize * model.PageIndex : model.TotalItems;
                    links.AppendFormat("<div class=\"col-sm-6 col-xs-12 hidden-xs\"><div class=\"dataTables_info\"> Hiển thị từ <span class=\"txt-color-darken\">{0}</span> đến <span class=\"txt-color-darken\">{1} trong <span class=\"text-primary\">{2}</span> bản ghi</div></div>", (model.PageIndex - 1) * model.PageSize + 1, lastEntry, model.TotalItems);
                }

                links.Append("<div class=\"col-xs-12 col-sm-6\"><div class=\"dataTables_paginate paging_simple_numbers\"><ul class=\"pagination pagination-sm\">");
                if (showFirst)
                {
                    if ((model.PageIndex >= 3) && (model.TotalPages > individualPagesDisplayedCount))
                    {
                        if (showIndividualPages)
                        {
                            links.Append("&nbsp;");
                        }
                        links.Append("<li class=\"first\">");
                        links.Append(CreatePageLink(1, "<<"));
                        links.Append("</li>");
                        //if ((showIndividualPages || (showPrevious && (model.PageIndex > 0))) || showLast)
                        //{
                        //    links.Append("&nbsp;...&nbsp;");
                        //}
                    }
                }
                if (showPrevious)
                {
                    if (model.PageIndex > 1)
                    {
                        links.Append("<li class=\"previous\">");
                        links.Append(CreatePageLink(model.PageIndex - 1, "<"));

                        if ((showIndividualPages || showLast) || (showNext && ((model.PageIndex + 1) < model.TotalPages)))
                        {
                            links.Append("&nbsp;");
                        }
                        links.Append("</li>");
                    }
                }
                if (showIndividualPages)
                {
                    int firstIndividualPageIndex = GetFirstIndividualPageIndex();
                    int lastIndividualPageIndex = GetLastIndividualPageIndex();
                    for (int i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
                    {
                        if (model.PageIndex == i + 1)
                        {
                            links.Append("<li class=\"active\">");
                            links.Append(CreatePageLink(i + 1, (i + 1).ToString()));
                            links.Append("</li>");
                        }
                        else
                        {
                            links.Append("<li>");
                            links.Append(CreatePageLink(i + 1, (i + 1).ToString()));
                            links.Append("</li>");
                        }
                        if (i < lastIndividualPageIndex)
                        {
                            links.Append("&nbsp;");
                        }
                    }
                }
                if (showNext)
                {
                    if ((model.PageIndex + 1) < model.TotalPages)
                    {
                        links.Append("<li class=\"next\">");
                        if (showIndividualPages)
                        {
                            links.Append("&nbsp;");
                        }

                        links.Append(CreatePageLink(model.PageIndex + 1, ">"));
                        links.Append("</li>");
                    }
                }
                if (showLast)
                {
                    if (((model.PageIndex + 3) < model.TotalPages) && (model.TotalPages > individualPagesDisplayedCount))
                    {
                        //if (showIndividualPages || (showNext && ((model.PageIndex + 1) < model.TotalPages)))
                        //{
                        //    links.Append("&nbsp;...&nbsp;");
                        //}
                        links.Append("<li class=\"last\">");
                        links.Append(CreatePageLink(model.TotalPages, ">>"));
                        links.Append("</li>");
                    }
                }

                links.Append("</ul></div></div></div>");
            }

            return MvcHtmlString.Create(links.ToString());
        }


        protected virtual int GetFirstIndividualPageIndex()
        {
            if ((model.TotalPages < individualPagesDisplayedCount) ||
                ((model.PageIndex - (individualPagesDisplayedCount / 2)) < 0))
            {
                return 0;
            }
            if ((model.PageIndex + (individualPagesDisplayedCount / 2)) >= model.TotalPages)
            {
                return (model.TotalPages - individualPagesDisplayedCount);
            }
            return (model.PageIndex - (individualPagesDisplayedCount / 2));
        }

        protected virtual int GetLastIndividualPageIndex()
        {
            int num = individualPagesDisplayedCount / 2;
            if ((individualPagesDisplayedCount % 2) == 0)
            {
                num--;
            }
            if ((model.TotalPages < individualPagesDisplayedCount) ||
                ((model.PageIndex + num) >= model.TotalPages))
            {
                return (model.TotalPages - 1);
            }
            if ((model.PageIndex - (individualPagesDisplayedCount / 2)) < 0)
            {
                return (individualPagesDisplayedCount - 1);
            }
            return (model.PageIndex + num);
        }

        protected virtual string CreatePageLink(int pageNumber, string text)
        {
            var builder = new TagBuilder("a");
            builder.SetInnerText(text);
            switch (text)
            {
                case "Previous": builder.AddCssClass("previous paginate_button");
                    break;
                case "Next": builder.AddCssClass("next paginate_button");
                    break;
                case "First": builder.AddCssClass("next paginate_button");
                    break;
                case "Last": builder.AddCssClass("next paginate_button");
                    break;
                default: builder.AddCssClass("paginate_button");
                    break;
            }
            builder.MergeAttribute("href", urlBuilder(pageNumber));
            return builder.ToString(TagRenderMode.Normal);
        }

        protected virtual string CreateDefaultUrl(int pageNumber)
        {
            var routeValues = new RouteValueDictionary();

            foreach (var key in viewContext.RequestContext.HttpContext.Request.QueryString.AllKeys.Where(key => key != null))
            {
                var value = viewContext.RequestContext.HttpContext.Request.QueryString[key];
                if (booleanParameterNames.Contains(key, StringComparer.InvariantCultureIgnoreCase))
                {
                    //little hack here due to ugly MVC implementation
                    //find more info here: http://www.mindstorminteractive.com/blog/topics/jquery-fix-asp-net-mvc-checkbox-truefalse-value/
                    if (!String.IsNullOrEmpty(value) && value.Equals("true,false", StringComparison.InvariantCultureIgnoreCase))
                    {
                        value = "true";
                    }
                }
                routeValues[key] = value;
            }

            routeValues[pageQueryName] = pageNumber;

            var url = UrlHelper.GenerateUrl(null, null, null, routeValues, RouteTable.Routes, viewContext.RequestContext, true);
            return url;
        }
    }
}