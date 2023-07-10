using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Adapter;
using MQTTnet.Diagnostics;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.Record.Chart;
using MQTTnet.Formatter;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Packets;

namespace RebarSampling
{


    public class MqttClientOpt
    {
        /// <summary>
        /// 发布者
        /// </summary>
        private IManagedMqttClient mqttClientPublisher = null;
        /// <summary>
        /// 订阅者
        /// </summary>
        private IManagedMqttClient mqttClientSubscriber = null;

        private string port = "1883";
        public async Task PublisherStart(string _server,string _port)
        {
            this.port = _port;

            var mqttfactory = new MqttFactory();

            var tlsOptions = new MqttClientTlsOptions
            {
                UseTls = false,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                AllowUntrustedCertificates = true
            };

            var options = new MqttClientOptions
            {
                ClientId = "",
                ProtocolVersion = MqttProtocolVersion.V311,
                ChannelOptions = new MqttClientTcpOptions
                {
                    //Server = "localhost",
                    Server = _server,
                    Port = int.Parse( this.port.Trim()),
                    TlsOptions = tlsOptions
                }

            };

            if (options.ChannelOptions == null)
            {
                throw new InvalidOperationException();
            }

            options.Credentials = new MqttClientCredentials("username", Encoding.UTF8.GetBytes("password"));
            options.CleanSession = true;
            options.KeepAlivePeriod = TimeSpan.FromSeconds(5);

            this.mqttClientPublisher = mqttfactory.CreateManagedMqttClient();
            this.mqttClientPublisher.ConnectedAsync += MqttClientPublisher_ConnectedAsync;
            this.mqttClientPublisher.DisconnectedAsync += MqttClientPublisher_DisconnectedAsync;
            this.mqttClientPublisher.ApplicationMessageReceivedAsync += MqttClientPublisher_ApplicationMessageReceivedAsync;

            await this.mqttClientPublisher.StartAsync(
                new ManagedMqttClientOptions
                {
                    ClientOptions = options
                });

        }

        public async Task PublisherStop()
        {
            if(this.mqttClientPublisher==null)
            {
                return;
            }
            await this.mqttClientPublisher.StopAsync();
            this.mqttClientPublisher = null;
        }

        public async Task Publish(string _topic,string _payload)
        {
            var payload =Encoding.UTF8.GetBytes(_payload);
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(_topic.Trim())
                .WithPayload(payload)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag()
                .Build();

            if(this.mqttClientPublisher != null)
            {
                await this.mqttClientPublisher.EnqueueAsync(message);
            }
        }

        public async Task Subscrib(string _topic)
        {
            var topicFilter = new MqttTopicFilter { Topic=_topic.Trim()};
            if(this.mqttClientSubscriber!=null)
            {
                await this.mqttClientSubscriber.SubscribeAsync(new List<MqttTopicFilter> { topicFilter });
            }
        }

        public async Task SubscriberStart(string _server, string _port)
        {
            this.port = _port;

            var mqttFactory = new MqttFactory();

            var tlsOptions = new MqttClientTlsOptions
            {
                UseTls = false,
                IgnoreCertificateChainErrors = true,
                IgnoreCertificateRevocationErrors = true,
                AllowUntrustedCertificates = true
            };

            var options = new MqttClientOptions
            {
                ClientId = "ClientSubscriber",
                ProtocolVersion = MqttProtocolVersion.V311,
                ChannelOptions = new MqttClientTcpOptions
                {
                    Server = _server,
                    Port = int.Parse(this.port.Trim()),
                    TlsOptions = tlsOptions
                }
            };

            if (options.ChannelOptions == null)
            {
                throw new InvalidOperationException();
            }

            options.Credentials = new MqttClientCredentials("username", Encoding.UTF8.GetBytes("password"));
            options.CleanSession = true;
            options.KeepAlivePeriod = TimeSpan.FromSeconds(5);

            this.mqttClientSubscriber = mqttFactory.CreateManagedMqttClient();
            this.mqttClientSubscriber.ConnectedAsync += MqttClientSubscriber_ConnectedAsync; 
            this.mqttClientSubscriber.DisconnectedAsync += MqttClientSubscriber_DisconnectedAsync; 
            this.mqttClientSubscriber.ApplicationMessageReceivedAsync += MqttClientSubscriber_ApplicationMessageReceivedAsync; 

            await this.mqttClientSubscriber.StartAsync(
                new ManagedMqttClientOptions
                {
                    ClientOptions = options
                });
        }
        public async Task SubscriberStop()
        {
            if (this.mqttClientSubscriber == null)
            {
                return;
            }
            await this.mqttClientSubscriber.StopAsync();
            this.mqttClientSubscriber = null;

        }
        private Task MqttClientSubscriber_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            string topicmsg = arg.ApplicationMessage.Topic;
            string payloadmsg = arg.ApplicationMessage.ConvertPayloadToString();
            string msg = $"Topic:{topicmsg}|Payload:{payloadmsg}";
            GeneralClass.interactivityData?.printlog(1,msg);
            GeneralClass.interactivityData?.mqttsubscribmsg(payloadmsg);
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private Task MqttClientSubscriber_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            GeneralClass.interactivityData?.printlog(1, "subscriber disconnected");
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private Task MqttClientSubscriber_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            GeneralClass.interactivityData?.printlog(1, "subscriber connected");
            return Task.CompletedTask;

            //throw new NotImplementedException();
        }

        private Task MqttClientPublisher_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            string topicmsg = arg.ApplicationMessage.Topic;
            string payloadmsg = arg.ApplicationMessage.ConvertPayloadToString();
            string msg = $"Topic:{topicmsg}|Payload:{payloadmsg}";
            GeneralClass.interactivityData?.printlog(1, msg);
            GeneralClass.interactivityData?.mqttpublishmsg(payloadmsg);
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }

        private Task MqttClientPublisher_DisconnectedAsync(MqttClientDisconnectedEventArgs arg)
        {
            GeneralClass.interactivityData?.printlog(1, "publisher disconnected");
            return Task.CompletedTask;

            //throw new NotImplementedException();
        }

        private Task MqttClientPublisher_ConnectedAsync(MqttClientConnectedEventArgs arg)
        {
            GeneralClass.interactivityData?.printlog(1, "publisher connected");
            return Task.CompletedTask;

            //throw new NotImplementedException();
        }
    }
}
