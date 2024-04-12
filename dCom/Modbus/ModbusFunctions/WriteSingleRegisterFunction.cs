using Common;
using Modbus.FunctionParameters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;

namespace Modbus.ModbusFunctions
{
    /// <summary>
    /// Class containing logic for parsing and packing modbus write single register functions/requests.
    /// </summary>
    public class WriteSingleRegisterFunction : ModbusFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteSingleRegisterFunction"/> class.
        /// </summary>
        /// <param name="commandParameters">The modbus command parameters.</param>
        public WriteSingleRegisterFunction(ModbusCommandParameters commandParameters) : base(commandParameters)
        {
            CheckArguments(MethodBase.GetCurrentMethod(), typeof(ModbusWriteCommandParameters));
        }

        /// <inheritdoc />
        public override byte[] PackRequest()
        {
            //TO DO: IMPLEMENT

            ModbusWriteCommandParameters parametri = CommandParameters as ModbusWriteCommandParameters;

            byte[] zahtev = new byte[12];

            zahtev[0] = BitConverter.GetBytes(parametri.TransactionId)[0];
            zahtev[1] = BitConverter.GetBytes(parametri.TransactionId)[1];
            zahtev[2] = BitConverter.GetBytes(parametri.ProtocolId)[0];
            zahtev[3] = BitConverter.GetBytes(parametri.ProtocolId)[1];
            zahtev[4] = BitConverter.GetBytes(parametri.Length)[0];
            zahtev[5] = BitConverter.GetBytes(parametri.Length)[1];
            zahtev[6] = parametri.UnitId;
            zahtev[7] = parametri.FunctionCode;
            zahtev[8] = BitConverter.GetBytes(parametri.OutputAddress)[0];
            zahtev[9] = BitConverter.GetBytes(parametri.OutputAddress)[1];
            zahtev[10] = BitConverter.GetBytes(parametri.Value)[0];
            zahtev[11] = BitConverter.GetBytes(parametri.Value)[1];

            return zahtev;
        }

        /// <inheritdoc />
        public override Dictionary<Tuple<PointType, ushort>, ushort> ParseResponse(byte[] zahtev)
        {
            //TO DO: IMPLEMENT

            Dictionary<Tuple<PointType, ushort>, ushort> zahtevRecnik = new Dictionary<Tuple<PointType, ushort>, ushort>();

            ushort izlaznaAdresa = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(zahtev, 8));
            ushort vrednost = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(zahtev, 10));

            zahtevRecnik.Add(new Tuple<PointType, ushort>(PointType.ANALOG_OUTPUT, izlaznaAdresa), vrednost);

            return zahtevRecnik;
        }
    }
}