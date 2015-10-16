namespace Demo.Numerics

open System

[<Struct>]
type Complex(real : float, imaginary : float) = 
    member this.Real = real
    member this.Imaginary = imaginary

    static member (+) (c1 : Complex, c2 : Complex) = 
        new Complex(c1.Real + c2.Real, c1.Imaginary + c2.Imaginary)

    static member (*) (c1 : Complex, c2 : Complex) = 
        new Complex(c1.Real * c2.Real - c1.Imaginary * c2.Imaginary,
                    c1.Real * c2.Imaginary + c1.Imaginary * c2.Real)

    member this.Magnitude = 
        sqrt(this.Real * this.Real + this.Imaginary * this.Imaginary)

    member private this.Connector = 
        if Math.Sign(this.Imaginary) > 0 then '+' else '\u2013'

    override this.ToString() = 
        sprintf "%f %c %f\u2148" this.Real this.Connector this.Imaginary

    member this.ToString(fmt : string) =
        sprintf "%s %c %s\u2148" (this.Real.ToString(fmt))
                                 this.Connector
                                 (this.Imaginary.ToString(fmt))

