﻿using System.Collections.Generic;
using System.Text;

namespace NuGet.Indexing
{
    public static class LuceneQueryCreator
    {
        public static string Parse(string nugetQuery)
        {
            if (string.IsNullOrWhiteSpace(nugetQuery))
            {
                return string.Empty;
            }

            StringBuilder luceneQuery = new StringBuilder();

            CreateFieldClause(luceneQuery, "Id", nugetQuery);
            CreateFieldClause(luceneQuery, "Version", nugetQuery);
            CreateFieldClause(luceneQuery, "TokenizedId", nugetQuery);
            CreateFieldClauseAND(luceneQuery, "TokenizedId", nugetQuery, 4);
            CreateFieldClause(luceneQuery, "ShingledId", nugetQuery);
            CreateFieldClause(luceneQuery, "Title", nugetQuery, 2);
            CreateFieldClauseAND(luceneQuery, "Title", nugetQuery, 4);
            CreateFieldClause(luceneQuery, "Tags", nugetQuery);
            CreateFieldClause(luceneQuery, "Description", nugetQuery);
            CreateFieldClause(luceneQuery, "Authors", nugetQuery);
            CreateFieldClause(luceneQuery, "Owners", nugetQuery);

            return luceneQuery.ToString();
        }

        static void CreateFieldClause(StringBuilder luceneQuery, string field, string query, float boost = 1.0f)
        {
            List<string> subterms = GetTerms(query);
            if (subterms.Count > 0)
            {
                if (subterms.Count == 1)
                {
                    luceneQuery.AppendFormat("{0}:{1}", field, subterms[0]);
                }
                else
                {
                    luceneQuery.AppendFormat("({0}:{1}", field, subterms[0]);
                    for (int i = 1; i < subterms.Count; i += 1)
                    {
                        luceneQuery.AppendFormat(" OR {0}:{1}", field, subterms[i]);
                    }
                    luceneQuery.Append(")");
                }

                if (boost != 1.0f)
                {
                    luceneQuery.AppendFormat("^{0} ", boost);
                }
                else
                {
                    luceneQuery.AppendFormat(" ");
                }
            }
        }

        private static void CreateFieldClauseAND(StringBuilder luceneQuery, string field, string query, float boost)
        {
            List<string> subterms = GetTerms(query);
            if (subterms.Count > 1)
            {
                luceneQuery.AppendFormat("({0}:{1}", field, subterms[0]);
                for (int i = 1; i < subterms.Count; i += 1)
                {
                    luceneQuery.AppendFormat(" AND {0}:{1}", field, subterms[i]);
                }
                luceneQuery.Append(')');
                if (boost != 1)
                {
                    luceneQuery.AppendFormat("^{0} ", boost);
                }
            }
        }

        private static List<string> GetTerms(string query)
        {
            List<string> result = new List<string>();

            if (query.StartsWith("\"") && query.EndsWith("\""))
            {
                result.Add(query);
            }
            else
            {
                bool literal = false;
                int start = 0;
                for (int i = 0; i < query.Length; i++)
                {
                    char ch = query[i];
                    if (ch == '"')
                    {
                        literal = !literal;
                    }
                    if (!literal)
                    {
                        if (ch == ' ')
                        {
                            string s = query.Substring(start, i - start);
                            if (!string.IsNullOrWhiteSpace(s))
                            {
                                result.Add(s);
                            }
                            start = i + 1;
                        }
                    }
                }

                string t = query.Substring(start, query.Length - start);
                if (!string.IsNullOrWhiteSpace(t))
                {
                    result.Add(t);
                }
            }

            return result;
        }

        private static string Quote(string term)
        {
            if (term.StartsWith("\"") && term.EndsWith("\""))
            {
                return term;
            }
            return string.Format("\"{0}\"", term);
        }
    }
}
