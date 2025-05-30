// Licensed to Elasticsearch B.V under one or more agreements.
// Elasticsearch B.V licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information

module CommandLine

open Argu
open Microsoft.FSharp.Reflection

type Arguments =
    | [<CliPrefix(CliPrefix.None);SubCommand>] Clean
    | [<CliPrefix(CliPrefix.None);SubCommand>] Build
    
    | [<CliPrefix(CliPrefix.None);Hidden;SubCommand>] PristineCheck 
    | [<CliPrefix(CliPrefix.None);Hidden;SubCommand>] GeneratePackages
    | [<CliPrefix(CliPrefix.None);Hidden;SubCommand>] ValidatePackages 
    | [<CliPrefix(CliPrefix.None);Hidden;SubCommand>] GenerateReleaseNotes 
    | [<CliPrefix(CliPrefix.None);Hidden;SubCommand>] GenerateApiChanges 
    | [<CliPrefix(CliPrefix.None);SubCommand>] Release
    
    | [<CliPrefix(CliPrefix.None);Hidden;SubCommand>] CreateReleaseOnGithub 
    | [<CliPrefix(CliPrefix.None);SubCommand>] Publish
    
    | [<Inherit;AltCommandLine("-s")>] SingleTarget of bool
    | [<Inherit>] Token of string 
with
    interface IArgParserTemplate with
        member this.Usage =
            match this with
            | Clean -> "clean known output locations"
            | Build -> "Run build and tests"
            | Release -> "runs build, and create an validates the packages shy of publishing them"
            | Publish -> "Runs the full release"
            
            | SingleTarget _ -> "Runs the provided sub command without running their dependencies"
            | Token _ -> "Token to be used to authenticate with github"
            
            | PristineCheck  
            | GeneratePackages
            | ValidatePackages 
            | GenerateReleaseNotes
            | GenerateApiChanges
            | CreateReleaseOnGithub 
                -> "Undocumented, dependent target"
    member this.Name =
        match FSharpValue.GetUnionFields(this, typeof<Arguments>) with
        | case, _ -> case.Name.ToLowerInvariant()
