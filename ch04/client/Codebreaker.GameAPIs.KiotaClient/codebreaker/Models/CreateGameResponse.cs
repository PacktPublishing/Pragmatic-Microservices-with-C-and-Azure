// <auto-generated/>
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Codebreaker.Client.Models
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.16.0")]
    #pragma warning disable CS1591
    public partial class CreateGameResponse : IParsable
    #pragma warning restore CS1591
    {
        /// <summary>The fieldValues property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public global::Codebreaker.Client.Models.CreateGameResponse_fieldValues? FieldValues { get; set; }
#nullable restore
#else
        public global::Codebreaker.Client.Models.CreateGameResponse_fieldValues FieldValues { get; set; }
#endif
        /// <summary>The gameType property</summary>
        public global::Codebreaker.Client.Models.GameType? GameType { get; set; }
        /// <summary>The id property</summary>
        public Guid? Id { get; set; }
        /// <summary>The maxMoves property</summary>
        public int? MaxMoves { get; set; }
        /// <summary>The numberCodes property</summary>
        public int? NumberCodes { get; set; }
        /// <summary>The playerName property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? PlayerName { get; set; }
#nullable restore
#else
        public string PlayerName { get; set; }
#endif
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Codebreaker.Client.Models.CreateGameResponse"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Codebreaker.Client.Models.CreateGameResponse CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Codebreaker.Client.Models.CreateGameResponse();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "fieldValues", n => { FieldValues = n.GetObjectValue<global::Codebreaker.Client.Models.CreateGameResponse_fieldValues>(global::Codebreaker.Client.Models.CreateGameResponse_fieldValues.CreateFromDiscriminatorValue); } },
                { "gameType", n => { GameType = n.GetEnumValue<global::Codebreaker.Client.Models.GameType>(); } },
                { "id", n => { Id = n.GetGuidValue(); } },
                { "maxMoves", n => { MaxMoves = n.GetIntValue(); } },
                { "numberCodes", n => { NumberCodes = n.GetIntValue(); } },
                { "playerName", n => { PlayerName = n.GetStringValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteObjectValue<global::Codebreaker.Client.Models.CreateGameResponse_fieldValues>("fieldValues", FieldValues);
            writer.WriteEnumValue<global::Codebreaker.Client.Models.GameType>("gameType", GameType);
            writer.WriteGuidValue("id", Id);
            writer.WriteIntValue("maxMoves", MaxMoves);
            writer.WriteIntValue("numberCodes", NumberCodes);
            writer.WriteStringValue("playerName", PlayerName);
        }
    }
}
