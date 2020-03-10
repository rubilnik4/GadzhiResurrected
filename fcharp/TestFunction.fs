namespace fcharp

module Say =
   let function1 x y = x + y

   let function2 (x: float) y = x + y

   let inline crossfoot (n:^a) : ^a =
       let zero:^a = LanguagePrimitives.GenericZero
       let ten:^a = (Seq.init 10 (fun _ -> LanguagePrimitives.GenericOne)) |> Seq.sum
       let rec compute (n:^a) =
           if n = zero then zero
           else ((n % ten):^a) + compute (n / ten)
       compute n
