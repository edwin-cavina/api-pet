using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LambdaPETUploadPhotos
{
	public class UploadModel
	{
		[JsonProperty(PropertyName = "img_base64")]
		public string ImgBase64 { get; set; }

		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
	}
}
