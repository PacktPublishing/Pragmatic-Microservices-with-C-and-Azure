# Chapter 2 - Minimal APIs: Creating REST Services

## Technical requirements

### Source code repo

The code for this chapter can be found in the following GitHub repository:
https://github.com/PacktPublishing/Pragmatic-Microservices-With-CSharp-and-Azure.

The source code folder ch02 contains the code samples for this chapter. You’ll find the code for these projects:

* Codebreaker.GamesAPIs – this is the Web API project.
* Codebreaker.GamesAPIs.Models – a library for the data models
* Codebreaker.GameAPIs.Algorithms – a library containing algorithms for the game
* Codebreaker.GamesAPIs.Algorithms.Tests – unit tests for the algorithms
* Codebreaker.AppHost - the host application for .NET Aspire
* Codebreaker.ServiceDefaults - service defaults for .NET Aspire

> You don’t implement the algorithms of the game in this chapter. The algorithms project is just for reference purposes, but you can simple use a NuGet package for the algorithms that has been made available for you to build the service.

### Development tools used

See [Installation](../installation.md) to install Visual Studio 2022 or Visual Studio Code and .NET Aspire. 

This chapter requires using Visual Studio 2022 or Visual Studio Code and .NET Aspire:

## Codebreaker diagrams

[Create a game](CreateAGame.md)

[Set a move](SetMove.md)