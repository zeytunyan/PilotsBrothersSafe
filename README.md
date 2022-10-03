# PilotsBrothersSafe

Реализация на C# головоломки, представленной в компьютерной игре-квесте "Братья пилоты".

В данной реализации поле может принимать разные размеры, от 3x3 до 10x10 (в оригинале было 4x4).



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


