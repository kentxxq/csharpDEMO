﻿// const string mirrorUrl = "https://dockerpull.com";

using System.Text.Json;
using DockerDistribution;

const string arch = "amd64";
const string os = "linux";
const string mirrorUrl = "https://dockerproxy.net";
// const string mirrorUrl = "https://registry-1.docker.io";
const string image = "library/nginx";
const string tag = "1.21.1";

var dockerClient = new DockerClient(mirrorUrl);
var manifestList = await dockerClient.GetManifestList(image, tag);
Console.WriteLine(JsonSerializer.Serialize(manifestList,Tools.MyJsonSerializerOptions));

// 拿到linux-x64 基础信息
var linuxX64BasicManifest = manifestList.Manifests.FirstOrDefault(x => x.Platform is { Architecture: arch, Os: os });
// 拿到linux-x64 详细信息, linuxX64BasicManifest!.Digest 就是dockerhub网站上的digest
var linuxX64DetailManifest = await dockerClient.GetManifest(image, linuxX64BasicManifest!.Digest);
Console.WriteLine(JsonSerializer.Serialize(linuxX64DetailManifest,Tools.MyJsonSerializerOptions));

// 下载layer,校验每一层的sha256
await dockerClient.DownloadImage(image, linuxX64DetailManifest);


// 记录下载速度
