using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace Pohui.Models
{
    public static class Utils
    {
        public static ICollection<Tag> GetTagsFromText(this string src)
        {
            var hashRegex = new Regex(@"#[A-z0-9-|_|А-я]+ ");
            var result = (from Match m in hashRegex.Matches(src)
                          select m.Groups[0].Value.Trim()).ToArray();
            ICollection<Tag> tags = result.Select(s => new Tag { Name = s }).ToList();
            return tags;
        }
    }
}