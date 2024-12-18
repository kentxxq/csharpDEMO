﻿using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using DockerDistribution.Models;

namespace DockerDistribution;

/// <summary>
/// 文档地址 https://distribution.github.io/distribution/spec/manifest-v2-2/
/// http文档地址 https://distribution.github.io/distribution/spec/api/
/// </summary>
public class DockerClient
{
    private readonly HttpClient _httpClient;
    private readonly string _registryUrl;

    public DockerClient(string registryUrl = "https://registry-1.docker.io", string username = "", string password = "",string httpProxy="")
    {
        _registryUrl = registryUrl;
        if (string.IsNullOrEmpty(httpProxy))
        {
            _httpClient = new HttpClient();
        }
        else
        {
            var handler = new HttpClientHandler
            {
                Proxy = StaticTools.GetWebproxyFromString(httpProxy),
                UseProxy = true
            };
            _httpClient =  new HttpClient(handler);
        }
        // _httpClient.DefaultRequestHeaders.Authorizat0ion = new AuthenticationHeaderValue("Basic", CreateBasicAuthHeader(username, password));
    }

    // 生成 Authorization Header
    // private string CreateBasicAuthHeader(string username, string password)
    // {
    //     var credentials = $"{username}:{password}";
    //     var base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));
    //     return base64Credentials;
    // }

    /// <summary>
    ///
    /// </summary>
    /// <param name="imageName">nginx</param>
    /// <param name="tag">1.21.1</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<ManifestList> GetManifestList(string imageName,string tag)
    {
        var manifestListUrl = $"{_registryUrl}/v2/{imageName}/manifests/{tag}";
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.docker.distribution.manifest.list.v2+json");

        var response = await _httpClient.GetAsync(manifestListUrl);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            throw new Exception("Failed to retrieve manifest list.");
        }
        var dockerManifestList = await response.Content.ReadFromJsonAsync<ManifestList>() ?? throw new Exception("Failed to deserialize manifest list.");
        _httpClient.DefaultRequestHeaders.Accept.Clear();

        return dockerManifestList;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="imageName">nginx</param>
    /// <param name="digest">sha256:123456</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<DockerManifest> GetManifest(string imageName,string digest)
    {
        var manifestUrl = $"{_registryUrl}/v2/{imageName}/manifests/{digest}";
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.docker.distribution.manifest.v2+json");

        var response = await _httpClient.GetAsync(manifestUrl);
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            throw new Exception("Failed to retrieve manifest list.");
        }
        var manifest = await response.Content.ReadFromJsonAsync<DockerManifest>() ?? throw new Exception("Failed to deserialize manifest.");
        _httpClient.DefaultRequestHeaders.Accept.Clear();

        return manifest;
    }

    /// <summary>
    /// 下载到当前目录,并校验sha256的值
    /// </summary>
    /// <param name="image">nginx</param>
    /// <param name="manifest"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<bool> DownloadImage(string image,DockerManifest manifest)
    {
        foreach (var layer in manifest.Layers)
        {
            var layerUrl = $"{_registryUrl}/v2/{image}/blobs/{layer.Digest}";
            _httpClient.DefaultRequestHeaders.Add("Accept",layer.MediaType);
            var layerResponse = await _httpClient.GetAsync(layerUrl);
            if (!layerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(await layerResponse.Content.ReadAsStringAsync());
                throw new Exception("Failed to retrieve layer.");
            }

            var layerDigest = layer.Digest.Split(":")[1];

            var layerFilePath = $"{layerDigest}.layer";
            await using var fileStream = new FileStream(layerFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
            await layerResponse.Content.CopyToAsync(fileStream);
            fileStream.Close();

            // 验证下载文件的哈希值
            await using var fileToHashStream = new FileStream(layerFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileHashBytes = await SHA256.HashDataAsync(fileToHashStream);
            Console.WriteLine(BitConverter.ToString(fileHashBytes).Replace("-", "").ToLower() == layerDigest ? $"{layerFilePath}校验通过" : $"{layerFilePath}校验失败");
        }
        return await Task.FromResult(true);
    }
}
