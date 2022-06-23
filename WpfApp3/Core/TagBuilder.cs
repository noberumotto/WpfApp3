using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.core.Tags;

namespace WpfApp3.core
{
    public class TagBuilder
    {
        public enum CharType
        {
            /// <summary>
            /// 小于 ＜
            /// </summary>
            Less,
            /// <summary>
            /// 大于 >
            /// </summary>
            Greater,
            /// <summary>
            /// 字符
            /// </summary>
            String,
            /// <summary>
            /// 空格
            /// </summary>
            Space,
            /// <summary>
            /// 等于 =
            /// </summary>
            Equalsign,
            /// <summary>
            /// 双引号 "
            /// </summary>
            Doublequote,
            /// <summary>
            /// 斜杠 /
            /// </summary>
            Slash

        }

        public enum State
        {
            Wait,
            Tag,
            Propertie,
            PropertieValue,
            Input,
            Content

        }
        private readonly string Text;
        private State state;
        private List<Tag> tags;
        public TagBuilder(string text)
        {
            Text = text;
            state = State.Wait;
            tags = new List<Tag>();
        }

        public List<Tag> GetTags()
        {
            return tags;
        }
        public void Render()
        {
            int index = 0;
            string token = "";
            var tag = new Tag();
            tag.Properties = new List<TagProperty>();

            TagProperty proptie = new TagProperty();

            while (index < Text.Length)
            {
                char c = Text[index];
                var cType = GetCharType(c);
                switch (state)
                {
                    case State.Wait:
                        switch (cType)
                        {
                            case CharType.Less:
                                state = State.Tag;
                                break;
                        }
                        break;

                    case State.Tag:
                        switch (cType)
                        {
                            case CharType.String:
                                token += c;
                                break;
                            case CharType.Space:
                                state = State.Propertie;
                                tag.Name = token;
                                token = "";
                                break;
                            case CharType.Greater:

                                break;
                        }
                        break;

                    case State.Propertie:
                        switch (cType)
                        {
                            case CharType.String:
                                token += c;
                                break;
                            case CharType.Equalsign:
                                state = State.PropertieValue;
                                index += 1;
                                proptie = new TagProperty();
                                proptie.Name = token;
                                token = "";
                                break;
                        }
                        break;
                    case State.PropertieValue:
                        switch (cType)
                        {
                            case CharType.Doublequote:
                                proptie.Value = token;
                                tag.Properties.Add(proptie);
                                token = "";
                                state = State.Input;
                                break;
                            case CharType.String:
                                token += c;
                                break;
                        }
                        break;
                    case State.Input:
                        switch (cType)
                        {
                            case CharType.Greater:
                                //  完成标签的开始部分
                                state = State.Content;
                                break;
                        }
                        break;
                    case State.Content:

                        switch (cType)
                        {
                            case CharType.Less:
                                if (Text[index + 1] == '/')
                                {
                                    //  结束标记
                                    tag.Content = token;

                                }
                                break;
                            default:

                                token += c;
                                break;
                        }
                        break;
                }
                index++;
            }

            tags.Add(tag);
        }

        private CharType GetCharType(char c)
        {
            if (c == '<')
            {
                return CharType.Less;
            }
            else if (c == '>')
            {
                return CharType.Greater;
            }
            else if (c == ' ')
            {
                return CharType.Space;
            }
            else if (c == '"')
            {
                return CharType.Doublequote;
            }
            else if (c == '=')
            {
                return CharType.Equalsign;
            }
            else if (c == '/')
            {
                return CharType.Slash;
            }
            else
            {
                return CharType.String;
            }
        }
    }
}
