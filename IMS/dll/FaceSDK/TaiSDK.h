

#define IN   
#define OUT


#define Tai_API(type) extern "C" _declspec(dllexport) type 




/*
初始化， 包括算法初始化和数据库初始化

输入:
	dbname, 数据库名称

输出:
	无

  返回: 
0   成功
-30 加载算法库失败
-40 数据库初始化失败
  */
Tai_API(int) face_init();



/*
关闭数据库，人脸算法卸载

  */
Tai_API(int) face_exit();


/*
从图像获取人脸特征

输入:
	pic_sec64, 前端控件采集的图像的base64串
	savePic,   图像保存文件名， 0表示不保存


输出:
	feature, 输出base64编码的人脸特征

  返回: 
>=0   成功
<0  失败
-30 加载算法库失败
-40 数据库初始化失败
-50 算法尚未初始化
  */

Tai_API(int) face_get_feature(char *pic_sec64,char feature[3000],char *savePic);


/*
从图像获取人脸特征

输入:
	fname,     图像文件名

输出:
	feature, 输出base64编码的人脸特征

  返回: 
>=0   成功
<0  失败
-30 加载算法库失败
-40 数据库初始化失败
-50 算法尚未初始化
  */
Tai_API(int) face_get_feature_from_image(char *fname,char feature[3000]);

/*
从图像获取人脸特征

输入:
	pic_sec64, 前端控件采集的图像的base64串
	savePic,   图像保存文件名， 0表示不保存


输出:
	feature, 输出base64编码的人脸特征

  返回: 
>=0   成功
<0  失败
-30 加载算法库失败
-40 数据库初始化失败
-50 算法尚未初始化
  */

Tai_API(int) face_get_feature(char *pic_sec64,char feature[3000],char *savePic);

/*
两个人脸特征进行对比

输入:
	feature1, 人脸特征
	feature2, 人脸特征


输出:
	无

  返回: 
  得分，最大127
>=0 成功
<0  失败
-30 加载算法库失败
-40 数据库初始化失败
-50 算法尚未初始化
  */
Tai_API(int) face_comp_feature(char feature1[3000],char feature2[3000]);



/*
两个人脸特征进行对比

输入:
	feature1, 人脸特征
	feature2, 人脸特征


输出:
	无

  返回: 
  得分，最大127
>=0 成功
<0  失败
-30 加载算法库失败
-40 数据库初始化失败
-50 算法尚未初始化
  */
typedef struct tagFace /* 先初始化为全0，再针对所需配置赋值    */
{
	int eyeDegree;			/* 人眼的睁开程度[0,100]，左右眼平均值   */
	int mouthDegree;		/* 嘴巴的张开程度[0,100]                 */
	int headPosi[3];		/* 歪头-转头-抬低头[-90,90]  */
	int age;				/* 年龄[1, 100]      */
	int gender;				/* 性别(1=男，2=女  */
	int occlusion;			/* 遮挡(1=不遮，2=挡)   */
	int fftSharpness;		/* 清晰度[1, 255]       */
	int hsvSkinRate;		/* 肤色占比，<30为灰度脸    */
	int rotEyeBall;			/* 眼球偏转度[0,100]，50居中    */
}Face_attr;


Tai_API(int) face_get_attrs(char *fname,Face_attr *p);


/*
两个人脸特征进行对比

输入:
	fname 文件名

输出:
	0 不存在人脸
	1 存在人脸
*/
Tai_API(int) face_exist(char *fname);



struct tagFaceCoord
{//能定人脸面积区域的两点C1(x1, y1), C2(x2, y2)
    int x1;
    int y1;
    int x2;
    int y2;
};

Tai_API(int) face_get_pos(IN char *fname,OUT tagFaceCoord faceArr[15]);








