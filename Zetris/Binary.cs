﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MisaMinoNET;

namespace Zetris {
    public static class Binary {
        static readonly int Version = 1;

        static byte[] CreateHeader() => Encoding.ASCII.GetBytes("ZETR").Concat(BitConverter.GetBytes(Version)).ToArray();

        static int DecodeHeader(BinaryReader reader) {
            if (!reader.ReadChars(4).SequenceEqual(new char[] { 'Z', 'E', 'T', 'R' })) throw new InvalidDataException();
            int version = reader.ReadInt32();

            if (version > Version) throw new InvalidDataException();

            return version;
        }

        public static MemoryStream EncodePreferences() {
            MemoryStream output = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(output)) {
                writer.Write(CreateHeader());

                writer.Write(Preferences.Styles.Count);
                foreach (Style style in Preferences.Styles)
                    WriteStyle(writer, style);

                writer.Write(Preferences.StyleIndex);
                writer.Write(Preferences.Speed);
                writer.Write(Preferences.Previews);
                writer.Write(Preferences.HoldAllowed);
                writer.Write(Preferences.PerfectClear);
                writer.Write(Preferences.C4W);
                writer.Write(Preferences.Player);
            }

            return output;
        }

        public static void DecodePreferences(FileStream input) {
            using (BinaryReader reader = new BinaryReader(input)) {
                int version = DecodeHeader(reader);

                if (version >= 1) {
                    Preferences.Styles = new List<Style>();

                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                        Preferences.Styles.Add(ReadStyle(reader));

                } else reader.ReadInt32();

                if (version >= 1)
                    Preferences.StyleIndex = reader.ReadInt32();

                Preferences.Speed = reader.ReadInt32();

                if (version >= 1) {
                    Preferences.Previews = reader.ReadInt32();
                    Preferences.HoldAllowed = reader.ReadBoolean();
                }

                Preferences.PerfectClear = reader.ReadBoolean();
                Preferences.C4W = reader.ReadBoolean();
                Preferences.Player = reader.ReadInt32();
            }
        }

        static void WriteStyle(BinaryWriter writer, Style style) {
            writer.Write(style.Name);
            writer.Write(style.Parameters.ToArray().SelectMany(BitConverter.GetBytes).ToArray());
        }

        static Style ReadStyle(BinaryReader reader) {
            string name = reader.ReadString();

            byte[] bytes = reader.ReadBytes(68);
            int[] param = new int[17];

            for (int j = 0; j < 17; j++)
                param[j] = BitConverter.ToInt32(bytes, j * 4);

            return new Style(name, MisaMinoParameters.FromArray(param));
        }

        public static MemoryStream EncodeStyle(Style style) {
            MemoryStream output = new MemoryStream();

            using (BinaryWriter writer = new BinaryWriter(output)) {
                writer.Write(CreateHeader());

                WriteStyle(writer, style);
            }

            return output;
        }

        public static Style DecodeStyle(FileStream input) {
            using (BinaryReader reader = new BinaryReader(input)) {
                int version = DecodeHeader(reader);

                return ReadStyle(reader);
            }
        }
    }
}