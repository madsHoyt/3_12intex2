import pandas as pd
import xgboost as xgb
# from sklearn.model_selection import train_test_split
# from sklearn.metrics import accuracy_score
# import numpy as np
import pickle

def load_pickle(file_name):
    with open(file_name, "rb") as f:
        model = pickle.load(f)
    return model

def make_prediction(model, headdirection, depth, facebundles, goods, haircolor, samplescollected, length, ageatdeath):
  # Create a DataFrame with the feature values
  data = [
      {
          "headdirection": headdirection,
          "depth": depth,
          "facebundles": facebundles,
          "goods": goods,
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
  cat_cols = ["headdirection", "facebundles", "goods", "haircolor", "samplescollected", "ageatdeath"]
  for col in cat_cols:
      df[col] = df[col].astype("category")

  # Create a DMatrix object from the DataFrame
  dtest = xgb.DMatrix(df, enable_categorical=True)

  # Make predictions
  y_pred = model.predict(dtest)

  reverse_mapping = {0: "Bones (No Wrapping)", 1: "Half Wrapping", 2: "Whole Wrapping"}

  # Convert the numerical prediction back to the original label
  label_pred = reverse_mapping[int(y_pred[0])]

  # Round the prediction
  return label_pred

model = load_pickle("C:/Users/kimba/OneDrive/Desktop/Python/final_wrappingXGB_XV2.pkl")

import sys

if __name__ == "__main__":
    headdirection = sys.argv[1]
    depth = float(sys.argv[2])
    facebundles = sys.argv[3]
    goods = sys.argv[4]
    haircolor = sys.argv[5]
    samplescollected = sys.argv[6]
    length = float(sys.argv[7])
    ageatdeath = sys.argv[8]

    prediction = make_prediction(model, headdirection, depth, facebundles, goods, haircolor, samplescollected, length, ageatdeath)
    print(prediction)
