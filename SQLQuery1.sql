﻿SELECT Lydmaling.Lyde, Lydmaling.Dato, Steder.Sted
FROM Lydmaling
INNER JOIN Steder ON Lydmaling.[FK IdSted]=Steder.IdSted;