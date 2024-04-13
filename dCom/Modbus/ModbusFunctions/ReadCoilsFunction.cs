using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus read coil functions/requests.
    /// </summary>
    public class ReadCoilsFunction : ModbusFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadCoilsFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
		public ReadCoilsFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
        {
            CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusReadCommandParameters));
        }

        /// <inheritdoc/>
        public override byte[] PackRequest()
        {
            //TO DO: IMPLEMENT

            ModbusReadCommandParameters parametri = CommandParameters as ModbusReadCommandParameters;

            byte[] zahtev = new byte[12];

            zahtev[0] = BitConverter.GetBytes(parametri.TransactionId)[1];
            zahtev[1] = BitConverter.GetBytes(parametri.TransactionId)[0];
            zahtev[2] = BitConverter.GetBytes(parametri.ProtocolId)[1];
            zahtev[3] = BitConverter.GetBytes(parametri.ProtocolId)[0];
            zahtev[4] = BitConverter.GetBytes(parametri.Length)[1];
            zahtev[5] = BitConverter.GetBytes(parametri.Length)[0];
            zahtev[6] = parametri.UnitId;
            zahtev[7] = parametri.FunctionCode;
            zahtev[8] = BitConverter.GetBytes(parametri.StartAddress)[1];
            zahtev[9] = BitConverter.GetBytes(parametri.StartAddress)[0];
            zahtev[10] = BitConverter.GetBytes(parametri.Quantity)[1];
            zahtev[11] = BitConverter.GetBytes(parametri.Quantity)[0];

            return zahtev;

        }

        /// <inheritdoc />
        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] response)
        {
            //TO DO: IMPLEMENT

            Dictionary<Tuple<PointType, ushort>, ushort> zahtevRecnik = new Dictionary<Tuple<PointType, ushort>, ushort>();

            int brojBitova = response[8];
            ushort startnaAdresa = ((ModbusReadCommandParameters)CommandParameters).StartAddress;
            ushort brojac = 0;

            for (int i = 0; i < brojBitova; i++)
            {

                byte temp = response[9 + i];
                byte maska = 1;

                ushort kolicina = ((ModbusReadCommandParameters)CommandParameters).Quantity;

                for (int j = 0; j < 8; j++)
                {

                    ushort vrednost = (ushort)(temp & maska);
                    zahtevRecnik.Add(new Tuple<PointType, ushort>(PointType.DIGITAL_OUTPUT, startnaAdresa++), vrednost);

                    temp >>= 1;
                    brojac++;

                    if (brojac >= kolicina)
                    {

                        break;
                    }

                }
            }

            return zahtevRecnik;
        }
    }
}