module MyLib.Core.Types

open Fable.Core

[<AttachMembers>]
type User (firstname : string, surname : string) =

    member _.Fullname = sprintf "%s %s" firstname surname

let inline add a b = a + b
