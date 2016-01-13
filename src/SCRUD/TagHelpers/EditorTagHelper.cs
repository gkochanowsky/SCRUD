﻿/*
	Created by Gene Kochanowsky

	Desc: This is a partial attempt to emulate some of the Editor Template functionality that is 
			present in Html Helpers as driven by the metadata decorating the model.

	History
	=========================================================================================
	2016/01/12	G.K.	Created.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.TagHelpers;
using Microsoft.AspNet.Mvc.ViewFeatures;
using Microsoft.AspNet.Razor.TagHelpers;


namespace SCRUD.TagHelpers
{
	[HtmlTargetElement("editor", Attributes = "asp-for", TagStructure = TagStructure.WithoutEndTag)]
	public class EditorTagHelper : InputTagHelper
	{
		public EditorTagHelper(IHtmlGenerator generator) : base(generator) { }

		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.TagName = "input";
			base.Process(context, output);

			var p = base.For;

			string v = null;
			string c = "";

			switch (p.ModelExplorer.ModelType.Name)
			{
				case "DateTime":    // Change format
					var m = (DateTime)p.Model;

					switch (p.Metadata.DataTypeName)
					{
						case "Date":
							v = "MM/dd/yyyy";
							c = "datepicker";
							break;
						case "DateTime":
							v = "MM/dd/yyyy hh:mm tt";
							c = "datetimepicker";
							break;
					}

					v = (m != default(DateTime) ? m.ToString(v) : "");
					break;
			}

			var cls = output.Attributes["class"].Value.ToString();

			if (!string.IsNullOrWhiteSpace(c) && !cls.Contains(c))
				output.Attributes["class"] = cls + " " + c;

			if (v != null)
				output.Attributes["value"] = v;
		}
	}
}
