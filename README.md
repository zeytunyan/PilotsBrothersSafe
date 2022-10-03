# PilotsBrothersSafe

Реализация на C# головоломки, представленной в компьютерной игре-квесте "Братья пилоты".

В данной реализации поле может принимать разные размеры, от 3x3 до 10x10 (в оригинале было 4x4).

====

## Правила
Цель игры - открыть сейф.

На сейфе множество рукояток, расположенных квадратом, как 2-мерный массив NxN.

Кликом мышки меняется положение рукоятки с вертикального в горизонтальное и обратно.

При повороте рукоятки поворачиваются все рукоятки в одной строке и в одном столбце. 

Сейф открывается, только если удается расположить все рукоятки параллельно друг другу (т.е. все вертикально или все горизонтально).

![image](https://user-images.githubusercontent.com/47988040/193587972-e00fc185-3850-4dff-9cba-458237e9990c.png)



## Подсказка
Если самостоятельно решить не получается, можно воспользоваться подсказкой и показать решение, нажав кнопку Solve. 

Отображаемое решение динамически изменяется и не является единственно верным, но часто, хоть и не всегда, бывает самым коротким.

![Solution_demonstration](https://user-images.githubusercontent.com/47988040/193585157-58efc9a0-63b9-4750-9f2a-85c5ac09bd66.png)



## Факты о решении

* Каждый поворот ручки имеет смысл не более одного раза (если ручка поворачивается дважды - она просто возвращается в исходное положение).
* Порядок поворотов не важен, для каждой ручки важно только то, сколько раз она была повернута. Для данной ручки люобй поворот любой другой ручки либо поворачивает данную, либо не затрагивает её, и, поскольку ручка может иметь только два положения, неважно, в какой последвательности были произведены повороты, важно лишь их количество (чётно оно или нечетно). 
* Если есть выигрышная комбинация ручек, которые надо повернуть, то комбинация всех ручек, не входящих в выигрушную комбинацию, также является выигрышной. Это вытекает из того факта, что все ручки в конечном итоге могут находиться как в вертикальном, так и в горизонтальном положении.
* Из вышесказанного следует, что при любой конфигурации поля головоломку можно решить не более чем за N<sup>2</sup>/2 ходов.






