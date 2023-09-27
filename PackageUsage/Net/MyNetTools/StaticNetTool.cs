using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using NetTools;
using SharpPcap;
using SharpPcap.LibPcap;

namespace MyNetTools;

public static class StaticNetTool
{
    /// <summary>
    /// 获取本机ipv4内网ip<br />
    /// 如果网络不可用返回127.0.0.1<br />
    /// 如果之前网络可用，可能会返回之前保留下来的ip地址
    /// </summary>
    /// <returns></returns>
    public static IPAddress GetLocalIP()
    {
        try
        {
            using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0);
            socket.Connect("223.5.5.5", 53);
            var endPoint = socket.LocalEndPoint as IPEndPoint;
            endPoint ??= new IPEndPoint(IPAddress.Loopback, 0);
            return endPoint.Address;
        }
        catch (Exception)
        {
            return IPAddress.Loopback;
        }
    }

    /// <summary>
    /// 判断是否内网ip
    /// </summary>
    /// <param name="testIp"></param>
    /// <returns></returns>
    public static bool IsInternal(string testIp)
    {
        if (testIp.StartsWith("::1"))
        {
            return true;
        }

        var ip = IPAddress.Parse(testIp).GetAddressBytes();
        return ip[0] switch
        {
            10 or 127 => true,
            172 => ip[1] >= 16 && ip[1] < 32,
            192 => ip[1] == 168,
            _ => false
        };
    }

    /// <summary>
    /// 根据当前的内网ipv4地址，来得到对应的mac地址
    /// </summary>
    /// <returns></returns>
    public static PhysicalAddress? GetLocalMac()
    {
        var dev = GetNetDevice();
        dev?.Open();
        var physicalAddress = dev?.MacAddress;
        dev?.Close();
        return physicalAddress;
    }

    /// <summary>
    /// 根据当前的内网ipv4地址，来得到对应的mac地址<br />
    /// 原理是使用的是抓包工具<br />
    /// ubuntu需要安装apt install libpcap-dev<br />
    /// windows需要安装pcap<br />
    /// 格式:AA-BB-CC-DD-EE-FF
    /// </summary>
    /// <returns></returns>
    public static string? GetLocalMacString()
    {
        var physicalAddress = GetLocalMac();
        if (physicalAddress != null)
        {
            return string.Join("-", physicalAddress.GetAddressBytes().Select(x => x.ToString("X2")).ToArray());
        }

        return null;
    }

    /// <summary>
    /// 获取当前ipv4的子网掩码
    /// </summary>
    /// <returns></returns>
    public static IPAddress GetNetMask()
    {
        var macAddress = GetLocalMac();
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (var nwif in networkInterfaces)
        {
            if (nwif.GetPhysicalAddress().ToString() == macAddress?.ToString())
            {
                foreach (var item in nwif.GetIPProperties().UnicastAddresses)
                {
                    if (item.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return item.IPv4Mask;
                    }
                }
            }
        }

        return IPAddress.Loopback;
    }

    /// <summary>
    /// 获取ipv4的网卡设备
    /// </summary>
    /// <returns></returns>
    public static LibPcapLiveDevice? GetNetDevice()
    {
        var ip = GetLocalIP().ToString();
        var devices = LibPcapLiveDeviceList.Instance;
        foreach (var dev in devices)
        {
            if (dev.ToString().Contains(ip))
            {
                return dev;
            }
        }

        return null;
    }

    /// <summary>
    /// npcap1.60版本on win11 https://github.com/nmap/npcap/issues/628
    /// 发送arp包，通过ip获取mac地址
    /// </summary>
    /// <param name="ip">ip地址</param>
    /// <returns></returns>
    public static PhysicalAddress? GetPhysicalAddressByIp(IPAddress ip)
    {
        var device = GetNetDevice();
        var arp = new ARP(device) { Timeout = TimeSpan.FromSeconds(2) };
        var address = arp.Resolve(ip);
        return address;
    }

    /// <summary>
    /// 获取本地网关地址
    /// </summary>
    /// <returns></returns>
    public static IPAddress? GetLocalGateway()
    {
        var device = GetNetDevice();
        if (device != null)
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var nwif in networkInterfaces)
            {
                var gateways = nwif.GetIPProperties().GatewayAddresses;
                foreach (var gateway in gateways)
                {
                    if (device.ToString().Contains(gateway.Address.ToString()))
                    {
                        return gateway.Address;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// 获取当前网段下的所有ip
    /// </summary>
    public static IEnumerable<IPAddress> GetAllSubnetIp()
    {
        var localIp = GetLocalIP();
        var netMask = GetNetMask();
        var range = new IPAddressRange(localIp, IPAddressRange.SubnetMaskLength(netMask));
        return range.AsEnumerable();
    }

    /// <summary>
    /// 拿到当前内网可以ping通的存活主机
    /// </summary>
    /// <returns></returns>
    public static List<IPAddress> GetAliveIpListByICMP()
    {
        var ips = GetAllSubnetIp().ToList();
        var aliveIps = new List<IPAddress>();
        var taskList = new List<Task<bool>>();
        foreach (var ip in ips)
        {
            taskList.Add(Task.Run(() => MyPing.StaticPing.PingIp(ip)));
        }

        Task.WhenAll(taskList.ToArray()).GetAwaiter().GetResult();
        for (var i = 0; i < taskList.Count; i++)
        {
            if (taskList[i].Result)
            {
                aliveIps.Add(ips[i]);
            }
        }

        return aliveIps;
    }

    /// <summary>
    /// 通过mac地址获取ip，基于ICMP遍历主机实现。速度很慢
    /// </summary>
    /// <param name="physicalAddress"></param>
    /// <returns></returns>
    public static IPAddress? GetIpByPhysicalAddress(PhysicalAddress physicalAddress)
    {
        #region rarp方式，但是通常路由器并未开启rarp模式

        //var device = GetNetDevice();
        //var localIp = GetLocalIP();
        //var localMacAddress = GetLocalMac();
        //var gatewayIp = GetLocalGateway();
        //var gatewayMacAddress = GetPhysicalAddressByIp(gatewayIp);
        //var ethernet = new EthernetPacket(localMacAddress, gatewayMacAddress, EthernetType.ReverseArp);
        //var rarp = new ArpPacket(ArpOperation.RequestReverse, physicalAddress, gatewayIp, localMacAddress, localIp);
        //ethernet.PayloadPacket = rarp;

        //device.Open(DeviceMode.Promiscuous);
        //var sendTime = DateTime.Now;

        //var sendTimes = 1;
        //ArpPacket responsePacket = null;
        //while ((DateTime.Now - sendTime).TotalSeconds < 5)
        //{
        //    var response = device.GetNextPacket();
        //    if (sendTimes > 1)
        //    {
        //        device.SendPacket(ethernet);
        //        sendTimes -= 1;
        //    }
        //    if (response != null)
        //    {
        //        responsePacket = Packet.ParsePacket(response.LinkLayerType, response.Data).Extract<ArpPacket>();
        //        if (responsePacket != null && responsePacket.Operation == ArpOperation.ReplyReverse)
        //        {
        //            break;
        //        }
        //    }
        //}
        //device.Close();
        //var xx = new ArpPacket(responsePacket.PayloadPacket.BytesSegment);

        //return xx.TargetProtocolAddress;

        #endregion rarp方式，但是通常路由器并未开启rarp模式

        #region 遍历存活主机来获取ip地址

        var aliveIps = GetAliveIpListByICMP();
        foreach (var aliveIp in aliveIps)
        {
            var macAddress = GetPhysicalAddressByIp(aliveIp);
            if (Equals(macAddress, physicalAddress))
            {
                return aliveIp;
            }
        }

        return null;

        #endregion 遍历存活主机来获取ip地址
    }
}