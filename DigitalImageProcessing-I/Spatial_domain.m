function Spatial_domain(I)

    M3=ones(3)/9;
    M7=ones(7)/49;
    M11=ones(11)/121;
    M15=ones(15)/225;
    M21=ones(21)/441;

    R0=imfilter(I,M3);
    R1=imfilter(I,M7);
    R2=imfilter(I,M11);
    R3=imfilter(I,M15);
    R4=imfilter(I,M21);

    subplot(3,4,1),imshow(I),title('Image');
    subplot(3,4,2),imshow(R0),title('R3');
    subplot(3,4,3),imshow(R1),title('R7');
    subplot(3,4,4),imshow(R2),title('R11');
    subplot(3,4,5),imshow(R3),title('R15');
    subplot(3,4,6),imshow(R4),title('R21');

    w3=log(abs(fftshift(fft2(M3,size(I,1), size(I,2))))+1);
    subplot(3,4,7),imshow(w3,[]),title('W3');

    w7=log(abs(fftshift(fft2(M7,size(I,1), size(I,2))))+1);
    subplot(3,4,8),imshow(w7,[]),title('W7');

    w11=log(abs(fftshift(fft2(M11,size(I,1), size(I,2))))+1);
    subplot(3,4,9),imshow(w11,[]),title('W11');

    w15=log(abs(fftshift(fft2(M15,size(I,1), size(I,2))))+1);
    subplot(3,4,10),imshow(w15,[]),title('W15');

    w21=log(abs(fftshift(fft2(M21,size(I,1), size(I,2))))+1);
    subplot(3,4,11),imshow(log(abs(w21)+1),[]),title('W21');

    % imwrite(M3,'Transformed images/Part II/M3.jpg');
    % imwrite(M7,'Transformed images/Part II/M7.jpg');
    % imwrite(M11,'Transformed images/Part II/M11.jpg');
    % imwrite(M15,'Transformed images/Part II/M15.jpg');
    % imwrite(M21,'Transformed images/Part II/M21.jpg');
    % imwrite(I,'Transformed images/Part II/Image.jpg');
    % imwrite(R0,'Transformed images/Part II/R3.jpg');
    % imwrite(R1,'Transformed images/Part II/R7.jpg');
    % imwrite(R2,'Transformed images/Part II/R11.jpg');
    % imwrite(R3,'Transformed images/Part II/R15.jpg');
    % imwrite(R4,'Transformed images/Part II/R21.jpg');
    % imwrite(w3,'Transformed images/Part II/w3.jpg');
    % imwrite(w7,'Transformed images/Part II/w7.jpg');
    % imwrite(w11,'Transformed images/Part II/w11.jpg');
    % imwrite(w15,'Transformed images/Part II/w15.jpg');
    % imwrite(w21,'Transformed images/Part II/w21.jpg');

end