module Bmp

open System
open System.IO
open System.Text
open Xamarin.Forms

let Create (width : int) (height : int) (getPixel : int -> int -> Color) =
    let headerSize = 54
    let numPixels = width * height
    let numPixelBytes = 4 * numPixels
    let fileSize = headerSize + numPixelBytes
    let buffer : byte array = Array.zeroCreate fileSize

    let memoryStream = new MemoryStream(buffer)
    let writer = new BinaryWriter(memoryStream, Encoding.UTF8)

    // Construct BMP header (14 bytes)
    do writer.Write 'B'             // Signature
       writer.Write 'M'
       writer.Write fileSize        // File size
       writer.Write (int16 0)       // Reserved
       writer.Write (int16 0)       // Reserved
       writer.Write headerSize      // Offset to pixels

    // Construct BitmapInfoHeader (40 bytes)
    do writer.Write 40              // Header size
       writer.Write width           // Pixel width
       writer.Write height          // Pixel height
       writer.Write (int16 1)       // Planes
       writer.Write (int16 32)      // Bits per pixel
       writer.Write 0               // Compression
       writer.Write numPixelBytes   // Image size in bytes
       writer.Write 0               // X pixels per meter
       writer.Write 0               // y pixels per meter
       writer.Write 0               // Number colors in color table
       writer.Write 0               // Important color names

    for row = 0 to height - 1 do
        for col = 0 to width - 1 do
            let color = getPixel row col
            let index = headerSize + 4 * (row * width + col)
            do buffer.[index + 0] <- byte (255.0 * color.B)
               buffer.[index + 1] <- byte (255.0 * color.G)
               buffer.[index + 2] <- byte (255.0 * color.R)
               buffer.[index + 3] <- byte (255.0 * color.A)

    do memoryStream.Position <- int64 0
    memoryStream :> Stream
