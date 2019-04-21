open FSharp.Compiler.Interactive.Shell
open System
open System.IO
open System.Text

let fsiSession = 
    let inStream = new StringReader("")
    let outStream = new StringWriter(StringBuilder())
    let errStream = new StringWriter(StringBuilder())
    let allArgs = [| "C:\\fsi.exe"; "--noninteractive" |]

    let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()
    FsiEvaluationSession.Create(fsiConfig, allArgs, inStream, outStream, errStream) 

let evalExpression<'T> text =
  match fsiSession.EvalExpression text with
  | Some value -> value.ReflectionValue |> unbox<'T>
  | None       -> failwith "Couldn't evaluate the expression"

let calc lambda nums =
    let f = evalExpression<int -> int> ("fun " + lambda)

    let m1 v = v + f 1
    let m2 v = v * f 10
    let m3 v = v + f 100

    let showStatus v m =
        printfn "Executing %s current value is %d" (m.GetType().Name) v
        m v

    let context results n = 
        List.fold showStatus n [m1; m2; m3] :: results

    nums |> List.fold context [] |> List.rev |> printfn "%A"

printf "Enter lambda to use: "
let lambdaString = Console.ReadLine()
calc lambdaString [ 10; 11; 13 ]
