using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsApi.Entities.PlacesFind.Request
{
    public enum InputType
    {
		[EnumMember(Value = "textquery")]
        TextQuery,
		[EnumMember(Value = "phonenumber")]
        PhoneNumber
    }
}
