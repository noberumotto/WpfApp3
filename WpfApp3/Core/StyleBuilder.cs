using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfApp3.Core.Style;

namespace WpfApp3.core
{
    public class StyleBuilder
    {
        public enum CharType
        {
            /// <summary>
            /// 点 .
            /// </summary>
            Dot,
            /// <summary>
            /// {
            /// </summary>
            Leftbraces,
            /// <summary>
            /// 字符
            /// </summary>
            String,
            /// <summary>
            /// }
            /// </summary>
            Rightbraces,
            /// <summary>
            /// ;
            /// </summary>
            Semicolon,
            /// <summary>
            /// :
            /// </summary>
            Colon

        }

        public enum State
        {
            Wait,
            Class,
            Propertie,
            PropertieValue,
            Input,
        }
        private readonly string Text;
        private State state;
        private List<Style> styles;
        public StyleBuilder(string text)
        {
            Text = text;
            state = State.Wait;
            styles = new List<Style>();
        }

        public List<Style> GetStyles()
        {
            return styles;
        }

        public void Render()
        {
            int index = 0;
            string token = "";
            var style = new Style();
            style.Properties = new List<StyleProperty>();

            StyleProperty proptie = new StyleProperty();

            while (index < Text.Length)
            {
                char c = Text[index];
                var cType = GetCharType(c);
                switch (state)
                {
                    case State.Wait:
                        switch (cType)
                        {
                            case CharType.Dot:
                                state = State.Class;
                                break;
                        }
                        break;

                    case State.Class:
                        switch (cType)
                        {
                            case CharType.String:
                                token += c;
                                break;
                            case CharType.Leftbraces:
                                state = State.Input;
                                token = Regex.Replace(token, @"(\r|\n)", "");
                                style.Name = token.Replace(" ", "");

                                token = "";
                                break;
                        }
                        break;

                    case State.Input:
                        switch (cType)
                        {
                            case CharType.String:
                                state = State.Propertie;
                                token += c;
                                break;
                            case CharType.Rightbraces:
                                style.Properties.Add(proptie);
                                break;
                        }
                        break;
                    case State.Propertie:
                        switch (cType)
                        {
                            case CharType.String:
                                token += c;
                                break;
                            case CharType.Colon:
                                state = State.PropertieValue;
                                index += 1;
                                proptie = new StyleProperty();

                                token = Regex.Replace(token, @"(\r|\n)", "");
                                token = token.Replace(" ", "");
                                proptie.Name = token;
                                token = "";
                                break;
                        }
                        break;
                    case State.PropertieValue:
                        switch (cType)
                        {
                            case CharType.Semicolon:
                                proptie.Value = token;
                                style.Properties.Add(proptie);
                                token = "";
                                state = State.Input;
                                break;
                            case CharType.String:
                                token += c;
                                break;
                        }
                        break;

                }
                index++;
            }

            styles.Add(style);
        }

        private CharType GetCharType(char c)
        {
            if (c == '.')
            {
                return CharType.Dot;
            }
            else if (c == '{')
            {
                return CharType.Leftbraces;
            }
            else if (c == '}')
            {
                return CharType.Rightbraces;
            }
            else if (c == ':')
            {
                return CharType.Colon;
            }
            else if (c == ';')
            {
                return CharType.Semicolon;
            }
            else
            {
                return CharType.String;
            }
        }
    }
}
