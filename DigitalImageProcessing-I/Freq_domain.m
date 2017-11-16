function Freq_domain(Image_src,Image_dst,r,fig)

    [x,y,~]=size(imread(Image_src));
    W=zeros(x,y);
    W(floor(x/2-(r*x)/2):floor(x/2-(r*x)/2+r*x),floor(y/2-(r*y)/2):floor(y/2-(r*y)/2+r*y))=1;
    I=im2double(rgb2gray(imread(Image_src)));

    F=fftshift(fft2(I));
    WF=F.*W;
    R=ifft2(ifftshift(WF));

    Image_dst=strcat(Image_dst,num2str(r));
    figure(fig);

    subplot(1,4,1),imshow(abs(I),[]),title('Image');
    imwrite(abs(I),strcat(Image_dst,'/Image.jpg'));

    subplot(1,4,2),imshow(log(abs(F)+1),[]),title('F');
    imwrite(log(abs(F)+1),strcat(Image_dst,'/F.jpg'));

    subplot(1,4,3),imshow(log(abs(WF)+1),[]),title('W*F');
    imwrite(log(abs(WF)+1),strcat(Image_dst,'/WxF.jpg'));

    subplot(1,4,4),imshow(abs(R),[]),title('R');
    imwrite(abs(R),strcat(Image_dst,'/R.jpg'));
    
end


