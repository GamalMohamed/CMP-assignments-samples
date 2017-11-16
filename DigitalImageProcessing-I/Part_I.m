Image_srcs={'Original images/High.jpg';'Original images/Low.jpg';'Original images/Moderate.jpg'};
Image_dsts={'Transformed images/Part I/High/Filter_';'Transformed images/Part I/Low/Filter_';'Transformed images/Part I/Moderate/Filter_'};

fig=1;
for image=1:3
    for r=0.1:0.2:0.9
        Freq_domain(Image_srcs{image},Image_dsts{image},r,fig);
        fig=fig+1;
    end
end