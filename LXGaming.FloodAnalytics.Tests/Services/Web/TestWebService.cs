﻿using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using LXGaming.Configuration;
using LXGaming.FloodAnalytics.Services.Web;
using NUnit.Framework;

namespace LXGaming.FloodAnalytics.Tests.Services.Web;

public class TestWebService : WebService {

    public TestWebService(IConfiguration configuration, JsonSerializerOptions jsonSerializerOptions) : base(
        configuration, jsonSerializerOptions) {
        jsonSerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
    }

    public override async Task<T> DeserializeAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default) {
        var expectedNode = await base.DeserializeAsync<JsonNode>(response, cancellationToken);
        return Deserialize<T>(expectedNode);
    }

    private T Deserialize<T>(JsonNode expectedNode) {
        Assert.That(expectedNode, Is.Not.Null);

        var actualObject = expectedNode.Deserialize<T>(JsonSerializerOptions);
        Assert.That(actualObject, Is.Not.Null);

        var actualNode = JsonSerializer.SerializeToNode<T>(actualObject!, JsonSerializerOptions);
        Assert.That(actualNode, Is.Not.Null);

        Warn.Unless(actualNode!.ToJsonString(), Is.EqualTo(expectedNode.ToJsonString()));

        return actualObject!;
    }
}