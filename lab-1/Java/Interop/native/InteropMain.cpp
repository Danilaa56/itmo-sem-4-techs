#include <InteropMain.h>

JNIEXPORT jint JNICALL Java_InteropMain_sum(JNIEnv *env, jclass, jintArray jArray) {
    int sum = 0;

    jsize len = env->GetArrayLength(jArray);
    jint *arr = env->GetIntArrayElements(jArray, nullptr);

    for (int i = 0; i < len; i++) {
        sum += arr[i];
    }

    env->ReleaseIntArrayElements(jArray, arr, 0);

    return sum;
}
