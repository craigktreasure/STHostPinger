using System;
using Newtonsoft.Json;

namespace SmartThingsHostPinger.Utils
{
	/// <summary>
	/// Deserialize to a concrete object.
	/// </summary>
	/// <typeparam name="TConcrete">The type of the t concrete.</typeparam>
	/// <seealso cref="Newtonsoft.Json.JsonConverter" />
	/// Code from https://greatrexpectations.com/2012/08/30/deserializing-interface-properties-using-json-net/.
	public class ConcreteTypeConverter<TConcrete> : JsonConverter
    {
		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		/// <param name="objectType">Type of the object.</param>
		/// <returns><c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.</returns>
		public override bool CanConvert(Type objectType)
        {
            // assume we can convert to anything for now
            return true;
        }

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		/// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value of object being read.</param>
		/// <param name="serializer">The calling serializer.</param>
		/// <returns>The object value.</returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // explicitly specify the concrete type we want to create
            return serializer.Deserialize<TConcrete>(reader);
        }

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		/// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The calling serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // use the default serialization - it works fine
            serializer.Serialize(writer, value);
        }
    }
}