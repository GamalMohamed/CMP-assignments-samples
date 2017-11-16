% Part 1
Features_Detection('images/dinosaur00.jpg','Features/Part 1/dinosaur00/');
Features_Detection('images/dinosaur01.jpg','Features/Part 1/dinosaur01/');

Features_Detection('images/mummy00.jpg','Features/Part 1/mummy00/');
Features_Detection('images/mummy01.jpg','Features/Part 1/mummy01/');

Features_Detection('images/scene.pgm','Features/Part 1/scene/');
Features_Detection('images/book.pgm','Features/Part 1/book/');
Features_Detection('images/box.pgm','Features/Part 1/box/');
Features_Detection('images/basmati.pgm','Features/Part 1/basmati/');

% Part 2
match('images/dinosaur00.jpg','images/dinosaur01.jpg');
title('Image matching');
saveas(gcf,'Features/Part2.jpg');

% Part 3
match('images/scene.pgm','images/book.pgm');
title('Object recognition I');
saveas(gcf,'Features/Part3_Scene&Book.jpg');

match('images/scene.pgm','images/box.pgm');
title('Object recognition II');
saveas(gcf,'Features/Part3_Scene&Box.jpg');

match('images/scene.pgm','images/basmati.pgm');
title('Object recognition III');
saveas(gcf,'Features/Part3_Scene&Basmati.jpg');
