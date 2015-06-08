# Floating Action Button for Xamarin.Forms

**This library requires AppCompat v21+**

A Xamarin.Forms wrapper for basic functionality of https://github.com/jamesmontemagno/FloatingActionButton-for-Xamarin.Android

And a Xamarin.Forms wrapper for a custom button on iOS

### Description

Floating action button for Xamarin.Forms.

```csharp
var layout = new RelativeLayout();
            
var normalFab = new FAB.Forms.FloatingActionButton();
normalFab.Source = "ic_add_white_24dp.png";
normalFab.Size = FabSize.Normal;

layout.Children.Add(
    normalFab,
    xConstraint: Constraint.RelativeToParent((parent) =>  { return (parent.Width - normalFab.Width) - 16; }),
    yConstraint: Constraint.RelativeToParent((parent) =>  { return (parent.Height - normalFab.Height) - 16; })
);

normalFab.SizeChanged += (sender, args) => { layout.ForceLayout(); };
```

Check the sample project to see additional examples.

### TODO Items

* Hide/Show FAB based on scroll of ScrollView, ListView or TableView
* Floating Action Menu


# License

The MIT License (MIT)

Copyright (c) 2015 Drew Frisk

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


This is comprised of James Montemagno's Xamarin.Android Floating Action Button: https://github.com/jamesmontemagno/FloatingActionButton-for-Xamarin.Android under MIT