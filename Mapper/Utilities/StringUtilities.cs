using ColossalFramework.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Mapper.Utilities
{
    class StringUtilities
    {
        public static Color ExtractColourFromTags(string text, Color defaultColour)
        {
            Regex colourExtraction = new Regex("(?:<color)(#[0-9a-fA-F]{3,6})(>.*)");
            string extractedTag = colourExtraction.Replace(text, "$1");

            if (extractedTag != null && extractedTag != text && extractedTag != "")
            {
                defaultColour = UIMarkupStyle.ParseColor(extractedTag, defaultColour);
            }

            return defaultColour;
        }

        public static string RemoveTags(string text)
        {
            Regex tagRemover = new Regex("(<\\/?color.*?>)");

            return tagRemover.Replace(text, "");
        }

        public static string WrapNameWithColorTags(string name, Color color)
        {
            string hexColour = UIMarkupStyle.ColorToHex(color);
            string returnName = "<color" + hexColour + ">" + name + "</color>";

            return returnName;
        }
    }
}
