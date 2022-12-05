Project Description

Expressive and powerfull test data generators.

**Example 1:** Generating cartesian product of values defined in arguments method One.Of()
```
   var rnd = new Random(0);
   int l = 3;
   A[] results = Generate.AllCombinations(i =>
      new A
      {
         P1 = One.Of(1, 2 * l, 3),
         P2 = new B
         {
            P1 = One.Of(1, 2) == 1 ? 2 : 3,
            P2 = "3",
            P3 = rnd.Next(20)
         }
      }).ToArray();
```

Code above generates:
```
P1: 1, P2: P1: 2, P2: 3, P3: 14
P1: 1, P2: P1: 3, P2: 3, P3: 16
P1: 6, P2: P1: 2, P2: 3, P3: 15
P1: 6, P2: P1: 3, P2: 3, P3: 11
P1: 3, P2: P1: 2, P2: 3, P3: 4
P1: 3, P2: P1: 3, P2: 3, P3: 11
```
**Example 2:** Calling API by all proper variants:
```
   Obj[] seq = Combine.AllCombinations<Obj>(cfg =>
   {
      cfg.OneOf(i => i.SetA(5), _ => _.A = 6);
      cfg.SetOneOf((i, val) => i.SetB(val), 1, 2, 3);
      cfg.OneOf(
         i => i.C = 4,
         i =>
         {
            if (i.A == 5 && i.B == 2)
               cfg.SkipCase();
            if (i.A == 6 && i.B == 2)
               i.C = 1;
            else
               i.C = 7;
         });
   }).ToArray();
```

Code above generates:
```
A: 5, B: 1, C: 4
A: 5, B: 1, C: 7
A: 5, B: 2, C: 4
A: 5, B: 3, C: 4
A: 5, B: 3, C: 7
A: 6, B: 1, C: 4
A: 6, B: 1, C: 7
A: 6, B: 2, C: 4
A: 6, B: 2, C: 1
A: 6, B: 3, C: 4
A: 6, B: 3, C: 7
```
Library can generate much more. All pair, random cases, ... See tests.
