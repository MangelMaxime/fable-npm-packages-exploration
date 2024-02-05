module Client.JavaScript

open MyLib.Core.Types

let user = User("John", "Doe")

printfn "Hello %s" user.Fullname

let user2 = JavaScript.Folder1.Util.createUser()

printfn "Hello %s" user2.Fullname
