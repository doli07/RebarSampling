using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Adapter;
using MQTTnet.Diagnostics;
using MQTTnet.Protocol;

namespace RebarSampling
{
    public class MqttServerOpt
    {
        private MqttServer mqttserver = null;

        public async Task StartMqttServer(string port)
        {
            if (mqttserver == null)
            {
                var options = new MqttServerOptions();
                options.DefaultEndpointOptions.Port = int.Parse(port);
                options.EnablePersistentSessions = true;

                this.mqttserver = new MqttFactory().CreateMqttServer(options);
                this.mqttserver.ValidatingConnectionAsync += Mqttserver_ValidatingConnectionAsync;

                try
                {
                    await this.mqttserver.StartAsync();

                }
                catch (Exception ex)
                {
                    await this.mqttserver.StopAsync();
                    this.mqttserver = null;
                }
            }
        }

        public async Task StopMqttServer()
        {
            if (this.mqttserver != null)
            {
                await this.mqttserver.StopAsync();
                this.mqttserver=null;
            }
        }

        private Task Mqttserver_ValidatingConnectionAsync(ValidatingConnectionEventArgs arg)
        {
            if (arg.ClientId.Length < 10)
            {
                arg.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                return Task.CompletedTask;
            }

            if (arg.Username != "username")
            {
                arg.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                return Task.CompletedTask;
            }
            if (arg.Password != "password")
            {
                arg.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                return Task.CompletedTask;
            }
            arg.ReasonCode = MqttConnectReasonCode.Success;
            return Task.CompletedTask;

            //throw new NotImplementedException();
        }
    }
}
