function Features_Detection(image_src,image_dst)
    I=imread(image_src);
    if ~ismatrix(I)
        I=rgb2gray(I);
    end
    
    figure
    imshow(I),title('Orginial image');
    saveas(gcf,strcat(image_dst,'Original.jpg'));
     
    % Canny
    image_canny = edge(I, 'canny');
    figure
    imshow(image_canny),hold on,title('Canny edge detector');
    saveas(gcf,strcat(image_dst,'Canny.jpg'));

    % Harris
    image_harris=corner(I,'Harris');
    figure
    imshow(I),hold on,title('Harris corner detector');
    plot(image_harris(:,1),image_harris(:,2),'r*');
    saveas(gcf,strcat(image_dst,'Harris.jpg'));

    % Hough
    [~,~,~,~,y5] = Hough_Grd(I);
    figure
    imshow(I);hold on,title('Hough lines detector');
    DrawLines_2Ends(y5);
    saveas(gcf,strcat(image_dst,'Hough.jpg'));

    % SIFT
    [Isift, ~, locs] = sift(image_src);
    showkeys(Isift, locs);
    title('SIFT');
    saveas(gcf,strcat(image_dst,'SIFT.jpg'));
    
end

