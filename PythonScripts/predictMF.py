import pandas as pd
import xgboost as xgb
import pickle

def load_pickle(file_name):
    with open(file_name, "rb") as f:
        model = pickle.load(f)
    return model

def make_predictionMF(model, headdirection, depth, facebundles, goods, wrapping, haircolor, samplescollected, length, ageatdeath):
    # print("headdirection:", headdirection)
    # print("depth:", depth)
    # print("facebundles:", facebundles)
    # print("goods:", goods)
    # print("wrapping:", wrapping)
    # print("haircolor:", haircolor)
    # print("samplescollected:", samplescollected)
    # print("length:", length)
    # print("ageatdeath:", ageatdeath)

  
    # Create a DataFrame with the feature values
    data = [
        {
            "headdirection": headdirection,
            "depth": depth,
            "facebundles": facebundles,
            "goods": goods,
            "wrapping": wrapping,
            "haircolor": haircolor,
            "samplescollected": samplescollected,
            "length": length,
            "ageatdeath": ageatdeath
        }
    ]
    df = pd.DataFrame(data)
    df["depth"] = df["depth"].astype(float)
    df["length"] = df["length"].astype(float)

    # Convert categorical columns to the "category" data type
    cat_cols = ["headdirection", "facebundles", "goods", "wrapping", "haircolor", "samplescollected", "ageatdeath"]
    for col in cat_cols:
        df[col] = df[col].astype("category")

    # Create a DMatrix object from the DataFrame
    dtest = xgb.DMatrix(df, enable_categorical=True)

    # Make predictions
    y_pred = model.predict(dtest)

    reverse_mapping = {0: "Female", 1: "Male"}

    # Convert the numerical prediction back to the original label
    label_pred = reverse_mapping[int(y_pred[0])]

    # Round the prediction
    return label_pred


# Load the XGBoost model from the pickle file
model = load_pickle("C:/Users/kimba/OneDrive/Desktop/Python/MF_XGB_XV2.pkl")

import sys

if __name__ == "__main__":
    headdirection = sys.argv[1]
    depth = float(sys.argv[2])
    facebundles = sys.argv[3]
    goods = sys.argv[4]
    wrapping = sys.argv[5]
    haircolor = sys.argv[6]
    samplescollected = sys.argv[7]
    length = float(sys.argv[8])
    ageatdeath = sys.argv[9]

    # Cast input values to the correct data types
    depth = float(depth)
    length = float(length)

    prediction2 = make_predictionMF(model, headdirection, depth, facebundles, goods, wrapping, haircolor, samplescollected, length, ageatdeath)
    print(prediction2)


