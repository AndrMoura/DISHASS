using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAMLEditor.Composite
{
    class Teste : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (destinationType != typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);

            List<INode> members = value as List<INode>;
            if (members == null)
                return "-";

            return string.Join(", ", members.Select(m => m));
            /*if (value is List<INode>)
            {
                return  "ASSSSSSSSSSSSSSD";
                Console.WriteLine("Chegou aki");
              //  return string.Join(",",((List<INode>) value).Select(x => x).ToString()); //FUNCIONA
              //  string.Format("Name: {0} - Address: {1}", contact.Name, contact.Address);
                //return string.Join(",", ((List<string>) value).Select(x => x));
            }
            Console.WriteLine("Chegou aki1");
            return base.ConvertTo(context, culture, value, destinationType);*/
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value,
            Attribute[] attributes)
        {
            List<PropertyDescriptor> list = new List<PropertyDescriptor>();
            List<INode> members = value as List<INode>;
            if (members != null)
            {
                foreach (INode member in members)
                {
                    if (member.Value != null)
                    {
                        list.Add(new MemberDescriptor(member, list.Count));

                    }
                }
            }
            return new PropertyDescriptorCollection(list.ToArray());
        }

        private class MemberDescriptor : SimplePropertyDescriptor
        {
            public MemberDescriptor(INode member, int index)
                : base(member.GetType(), index.ToString(), typeof(string))
            {
                Member = member;
            }

            public INode Member { get; private set; }

            public override object GetValue(object component)
            {
                return Member.Value;
            }

            public override void SetValue(object component, object value)
            {
                Member.Value = (string)value;
            }
        }
    }
}