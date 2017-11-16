high=im2double(rgb2gray(imread('Original images/High.jpg')));
low=im2double(rgb2gray(imread('Original images/Low.jpg')));
moderate=im2double(rgb2gray(imread('Original images/Moderate.jpg')));

figure(1)
Spatial_domain(high);
figure(2)
Spatial_domain(low);
figure(3)
Spatial_domain(moderate);

